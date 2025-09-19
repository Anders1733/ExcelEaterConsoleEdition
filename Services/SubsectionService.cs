using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelEaterConsoleEdition.Services
{
    public class SubsectionService
    {
        public static async Task ImportSubsectionsFromExcelToDb(ApplicationDbContext dbContext, List<List<object>> subsectionNames)
        {
            // Перебираем строки Excel-данных (предполагая, что первый элемент в каждом списке - это название раздела)
            foreach (var row in subsectionNames)
            {
                // Получаем название раздела из первой ячейки строки
                var subsectionName = row.Count > 0 ? row[0]?.ToString().Trim() : null;

                if (!string.IsNullOrWhiteSpace(subsectionName)) // проверяем, что название не пустое
                {
                    // Проверяем, существует ли уже такой раздел в базе данных
                    var existingSubsection = await dbContext.Subsections.FirstOrDefaultAsync(s => s.Name == subsectionName);

                    if (existingSubsection == null)
                    {
                        // Если такого раздела нет, добавляем новую запись
                        var newSubsection = new SubsectionEntity
                        {
                            Name = subsectionName
                        };

                        dbContext.Subsections.Add(newSubsection);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}

