using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelEaterConsoleEdition.Services
{
    public class UnitService
    {
        public static async Task ImportUnitsFromExcelToDb(ApplicationDbContext dbContext, List<List<object>> unitNames)
        {
            // Перебираем строки Excel-данных (предполагая, что первый элемент в каждом списке - это название раздела)
            foreach (var row in unitNames)
            {
                // Получаем название раздела из первой ячейки строки
                var unitName = row.Count > 0 ? row[0]?.ToString().Trim() : null;

                if (!string.IsNullOrWhiteSpace(unitName)) // проверяем, что название не пустое
                {
                    // Проверяем, существует ли уже такой раздел в базе данных
                    var existingUnits = await dbContext.Units.FirstOrDefaultAsync(s => s.Name == unitName);

                    if (existingUnits == null)
                    {
                        // Если такого раздела нет, добавляем новую запись
                        var newSubsection = new UnitEntity
                        {
                            Name = unitName
                        };

                        dbContext.Units.Add(newSubsection);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
