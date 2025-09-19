using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExcelEaterConsoleEdition.Services
{
    public class CompetencyService
    {
        public static async Task ImportCompetenciesFromExcelToDb(ApplicationDbContext dbContext, List<List<object>> dataRows)
        {
            foreach (var row in dataRows)
            {

                var existingEmployeeId = await FindEmployeeIdByName(row[0].ToString(), row[1].ToString(), dbContext);
                var existingDirectionId = await FindDirectionIdByName(row[2].ToString(), dbContext);
                var existingSectionId = await FindSectionIdByName(row[3].ToString(), dbContext);
                var existingSubsectionId = await FindSubsectionIdByName(row[4].ToString(), dbContext);
                var existingTopicId = await FindTopicIdByName(row[5].ToString(), dbContext);
                var currentLevel = Int32.Parse(row[6].ToString());
                var desiredLevel = Int32.Parse(row[7].ToString());

                // Проверяем, существует ли уже подобная запись в базе данных
                var existingCompetency = await dbContext.Competencies.FirstOrDefaultAsync(
                    c =>
                        c.EmployeeId == existingEmployeeId &&
                        c.DirectionId == existingDirectionId &&
                        c.SectionId == existingSectionId &&
                        c.SubsectionId == existingSubsectionId &&
                        c.TopicId == existingTopicId
                );

                if (existingCompetency != null)
                {
                    // Обновляем уровни, если они изменились
                    if (currentLevel != existingCompetency.CurrentLevel || desiredLevel != existingCompetency.DesiredLevel)
                    {
                        existingCompetency.CurrentLevel = currentLevel;
                        existingCompetency.DesiredLevel = desiredLevel;

                        // Сохраняем изменения в базе данных
                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    // Записи нет, создаем новую
                    var newCompetency = new CompetencyEntity
                    {
                        EmployeeId = existingEmployeeId,
                        DirectionId = existingDirectionId,
                        SectionId = existingSectionId,
                        SubsectionId = existingSubsectionId,
                        TopicId = existingTopicId,
                        CurrentLevel = currentLevel,
                        DesiredLevel = desiredLevel
                    };

                    dbContext.Competencies.Add(newCompetency);
                    await dbContext.SaveChangesAsync();
                }
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

        private static async Task<int> FindDirectionIdByName(string directionName, ApplicationDbContext dbContext)
        {
            // Поиск существующей записи сотрудника по имени
            var existingDirection = await dbContext.Directions
                                                  .FirstOrDefaultAsync(e => e.Name == directionName);

            if (existingDirection != null)
            {
                return existingDirection.DirectionId;
            }

            var newDirection = new DirectionEntity
            {
                Name = directionName
            };

            dbContext.Directions.Add(newDirection);

            await dbContext.SaveChangesAsync();

            return newDirection.DirectionId;
        }

        private static async Task<int> FindSectionIdByName(string sectionName, ApplicationDbContext dbContext)
        {
            // Поиск существующей записи сотрудника по имени
            var existingSection = await dbContext.Sections
                                                  .FirstOrDefaultAsync(e => e.Name == sectionName);

            if (existingSection != null)
            {
                return existingSection.SectionId;
            }

            var newSection = new SectionEntity
            {
                Name = sectionName
            };

            dbContext.Sections.Add(newSection);

            await dbContext.SaveChangesAsync();

            return newSection.SectionId;
        }

        private static async Task<int> FindSubsectionIdByName(string subsectionName, ApplicationDbContext dbContext)
        {
            // Поиск существующей записи сотрудника по имени
            var existingSubsection = await dbContext.Subsections
                                                  .FirstOrDefaultAsync(e => e.Name == subsectionName);

            if (existingSubsection != null)
            {
                return existingSubsection.SubsectionId;
            }

            var newSubsection = new SubsectionEntity
            {
                Name = subsectionName
            };

            dbContext.Subsections.Add(newSubsection);

            await dbContext.SaveChangesAsync();

            return newSubsection.SubsectionId;
        }

        private static async Task<int> FindTopicIdByName(string topicName, ApplicationDbContext dbContext)
        {
            // Поиск существующей записи сотрудника по имени
            var existingTopic = await dbContext.Topics
                                                  .FirstOrDefaultAsync(e => e.Name == topicName);

            if (existingTopic != null)
            {
                return existingTopic.TopicId;
            }

            var newTopic = new TopicEntity
            {
                Name = topicName
            };

            dbContext.Topics.Add(newTopic);

            await dbContext.SaveChangesAsync();

            return newTopic.TopicId;
        }
    }
}
