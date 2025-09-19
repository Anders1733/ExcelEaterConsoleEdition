using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelEaterConsoleEdition.Services
{
    public class SectionService
    {

        public static async Task ImportSectionsFromExcelToDb(ApplicationDbContext dbContext, List<List<object>> sectionNames)
        {
            // Перебираем строки Excel-данных (предполагая, что первый элемент в каждом списке - это название раздела)
            foreach (var row in sectionNames)
            {
                // Получаем название раздела из первой ячейки строки
                var sectionName = row.Count > 0 ? row[0]?.ToString().Trim() : null;

                if (!string.IsNullOrWhiteSpace(sectionName)) // проверяем, что название не пустое
                {
                    // Проверяем, существует ли уже такой раздел в базе данных
                    var existingSection = await dbContext.Sections.FirstOrDefaultAsync(s => s.Name == sectionName);

                    if (existingSection == null)
                    {
                        // Если такого раздела нет, добавляем новую запись
                        var newSection = new SectionEntity
                        {
                            Name = sectionName
                        };

                        dbContext.Sections.Add(newSection);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
