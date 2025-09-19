using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelEaterConsoleEdition.Services
{
    public class DirectionService
    {
        public static async Task ImportDirectionsFromExcelToDb(ApplicationDbContext dbContext, List<List<object>> directionNames)
        {
            // Перебираем строки Excel-данных (предполагая, что первый элемент в каждом списке - это название раздела)
            foreach (var row in directionNames)
            {
                // Получаем название раздела из первой ячейки строки
                var directionName = row.Count > 0 ? row[0]?.ToString().Trim() : null;

                if (!string.IsNullOrWhiteSpace(directionName)) // проверяем, что название не пустое
                {
                    // Проверяем, существует ли уже такой раздел в базе данных
                    var existingDirection = await dbContext.Directions.FirstOrDefaultAsync(s => s.Name == directionName);

                    if (existingDirection == null)
                    {
                        // Если такого раздела нет, добавляем новую запись
                        var newDirection = new DirectionEntity
                        {
                            Name = directionName
                        };

                        dbContext.Directions.Add(newDirection);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
