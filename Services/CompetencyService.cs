using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.Entities;
using ExcelEaterConsoleEdition.Parser;
using ExcelEaterConsoleEdition.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;


namespace ExcelEaterConsoleEdition.Services
{
    public class CompetencyService
    {

        public static async Task ImportCompetenciesFromExcelToDb(ApplicationDbContext dbContext, string filePath)
        {
            Dictionary<string, EmployeeEntity> employees = new(); // Сотрудники
            Dictionary<string, DirectionEntity> directions = new(); // Направления
            Dictionary<string, SectionEntity> sections = new(); // Секции
            Dictionary<string, SubsectionEntity> subsections = new(); // Подразделы
            Dictionary<string, TopicEntity> topics = new(); // Темы

            // Сначала проверяем базу данных на наличие записей
            bool hasEmployees = await dbContext.Employees.AnyAsync();
            bool hasDirections = await dbContext.Directions.AnyAsync();
            bool hasSections = await dbContext.Sections.AnyAsync();
            bool hasSubsections = await dbContext.Subsections.AnyAsync();
            bool hasTopics = await dbContext.Topics.AnyAsync();

            // Заполняем словари только если соответствующие таблицы содержат данные
            if (hasEmployees)
                employees = await dbContext.Employees.ToDictionaryAsync(e => e.Name);
            if (hasDirections)
                directions = await dbContext.Directions.ToDictionaryAsync(d => d.Name);
            if (hasSections)
                sections = await dbContext.Sections.ToDictionaryAsync(s => s.Name);
            if (hasSubsections)
                subsections = await dbContext.Subsections.ToDictionaryAsync(ss => ss.Name);
            if (hasTopics)
                topics = await dbContext.Topics.ToDictionaryAsync(t => t.Name);

            var existingEmployeeId = await FindEmployeeIdByName(
                ExcelHelper.ReadCellValue(filePath, LaunchParameters.LaunchParameters.LEGEND_SHEET_POSITION, LaunchParameters.LaunchParameters.FULL_NAME_POSITION),
                ExcelHelper.ReadCellValue(filePath, LaunchParameters.LaunchParameters.LEGEND_SHEET_POSITION, LaunchParameters.LaunchParameters.UNIT_POSITION),
                dbContext);

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                foreach (var sheetIndex in LaunchParameters.LaunchParameters.SheetNumbersToParse)
                {
                    var existingDirectionId = await FindDirectionIdByName(ExcelHelper.GetSheetNameBySheetIndex(filePath, sheetIndex), dbContext, directions);
                    List<List<object>> dataRows = ExcelHelper.ImportSingleSheetToList(filePath, sheetIndex);
                    try
                    {
                        foreach (var row in dataRows)
                        {
                            var sectionName = row[0].ToString();
                            var subsectionName = row[1].ToString();
                            var topicName = row[2].ToString();
                            var currentLevel = int.Parse(row[3].ToString());
                            var desiredLevel = int.Parse(row[4].ToString());

                            // Получение ID секций, подразделов и тем из словаря или из базы данных
                            var existingSectionId = await FindSectionIdByName(sectionName, dbContext, sections);
                            var existingSubsectionId = await FindSubsectionIdByName(subsectionName, dbContext, subsections);
                            var existingTopicId = await FindTopicIdByName(topicName, dbContext, topics);

                            // Проверяем наличие записи в базе данных
                            var existingCompetency = await dbContext.Competencies.FirstOrDefaultAsync(c =>
                                c.EmployeeId == existingEmployeeId &&
                                c.DirectionId == existingDirectionId &&
                                c.SectionId == existingSectionId &&
                                c.SubsectionId == existingSubsectionId &&
                                c.TopicId == existingTopicId);

                            UpdateOrCreateCompetency(existingCompetency, dbContext, existingEmployeeId, existingDirectionId, existingSectionId, existingSubsectionId, existingTopicId, currentLevel, desiredLevel);
                            
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine("Ошибка при сохранении изменений: ");
                        Console.WriteLine(ex.InnerException.Message); // Сообщение внутренней ошибки
                        Console.WriteLine(ex.StackTrace); // Трассировка стека
                        throw;
                    }
                }
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        private static void UpdateOrCreateCompetency(
            CompetencyEntity competency,
            ApplicationDbContext dbContext,
            int employeeId,
            int directionId,
            int sectionId,
            int subsectionId,
            int topicId,
            int currentLevel,
            int desiredLevel)
        {
            if (competency != null)
            {
                competency.CurrentLevel = currentLevel;
                competency.DesiredLevel = desiredLevel;
            }
            else
            {
                dbContext.Competencies.Add(new CompetencyEntity
                {
                    EmployeeId = employeeId,
                    DirectionId = directionId,
                    SectionId = sectionId,
                    SubsectionId = subsectionId,
                    TopicId = topicId,
                    CurrentLevel = currentLevel,
                    DesiredLevel = desiredLevel
                });
            }
        }


        private static async Task<int> FindEmployeeIdByName(string employeeName, string unitName, ApplicationDbContext dbContext)
        {
            // Поиск существующей записи сотрудника по имени
            var existingEmployee = await dbContext.Employees
                                                  .FirstOrDefaultAsync(e => e.Name == employeeName);

            if (existingEmployee != null)
            {
                // Сотрудник существует, возвращаем его EmployeeId
                return existingEmployee.EmployeeId;
            }

            // Сотрудника не нашли, создаем новую запись
            var newEmployee = new EmployeeEntity
            {
                Name = employeeName,
                EmployeeIdInVbpm = "", // Пока оставляем пустой email
                UnitId = await FindUnitIdByName(unitName, dbContext),
            };

            // Добавляем нового сотрудника в контекст
            dbContext.Employees.Add(newEmployee);

            // Сохраняем изменения в базе данных
            await dbContext.SaveChangesAsync();

            // Возвращаем вновь созданный EmployeeId
            return newEmployee.EmployeeId;
        }

        private static async Task<int> FindUnitIdByName(string unitName, ApplicationDbContext dbContext)
        {
            // Поиск существующей записи сотрудника по имени
            var existingEmployee = await dbContext.Units
                                                  .FirstOrDefaultAsync(e => e.Name == unitName);

            if (existingEmployee != null)
            {
                return existingEmployee.UnitId;
            }

            var newUnit = new UnitEntity
            {
                Name = unitName
            };

            dbContext.Units.Add(newUnit);

            await dbContext.SaveChangesAsync();

            return newUnit.UnitId;
        }

        private static async Task<int> FindDirectionIdByName(string directionName, ApplicationDbContext dbContext, Dictionary<string, DirectionEntity> directions)
        {
            if (directions.ContainsKey(directionName))
            {
                return directions[directionName].DirectionId;
            }

            var foundDirection = await dbContext.Directions.FirstOrDefaultAsync(d => d.Name == directionName);

            if (foundDirection != null)
            {
                directions.Add(foundDirection.Name, foundDirection);
                return foundDirection.DirectionId;
            }
            else
            {
                var newDirection = new DirectionEntity { Name = directionName };
                dbContext.Directions.Add(newDirection);
                await dbContext.SaveChangesAsync();

                directions.Add(newDirection.Name, newDirection);
                return newDirection.DirectionId;
            }
        }

        private static async Task<int> FindSectionIdByName(string sectionName, ApplicationDbContext dbContext, Dictionary<string, SectionEntity> sections)
        {
            if (sections.ContainsKey(sectionName))
            {
                return sections[sectionName].SectionId;
            }

            var foundSection = await dbContext.Sections.FirstOrDefaultAsync(s => s.Name == sectionName);

            if (foundSection != null)
            {
                sections.Add(foundSection.Name, foundSection);
                return foundSection.SectionId;
            }
            else
            {
                var newSection = new SectionEntity { Name = sectionName };
                dbContext.Sections.Add(newSection);
                await dbContext.SaveChangesAsync();

                sections.Add(newSection.Name, newSection);
                return newSection.SectionId;
            }
        }

        private static async Task<int> FindSubsectionIdByName(string subsectionName, ApplicationDbContext dbContext, Dictionary<string, SubsectionEntity> subsections)
        {
            if (subsections.ContainsKey(subsectionName))
            {
                return subsections[subsectionName].SubsectionId;
            }

            var foundSubsection = await dbContext.Subsections.FirstOrDefaultAsync(ss => ss.Name == subsectionName);

            if (foundSubsection != null)
            {
                subsections.Add(foundSubsection.Name, foundSubsection);
                return foundSubsection.SubsectionId;
            }
            else
            {
                var newSubsection = new SubsectionEntity { Name = subsectionName };
                dbContext.Subsections.Add(newSubsection);
                await dbContext.SaveChangesAsync();

                subsections.Add(newSubsection.Name, newSubsection);
                return newSubsection.SubsectionId;
            }
        }

        private static async Task<int> FindTopicIdByName(string topicName, ApplicationDbContext dbContext, Dictionary<string, TopicEntity> topics)
        {
            if (topics.ContainsKey(topicName))
            {
                return topics[topicName].TopicId;
            }

            var foundTopic = await dbContext.Topics.FirstOrDefaultAsync(t => t.Name == topicName);

            if (foundTopic != null)
            {
                topics.Add(foundTopic.Name, foundTopic);
                return foundTopic.TopicId;
            }
            else
            {
                var newTopic = new TopicEntity { Name = topicName };
                dbContext.Topics.Add(newTopic);
                await dbContext.SaveChangesAsync();

                topics.Add(newTopic.Name, newTopic);
                return newTopic.TopicId;
            }
        }

    }
}
