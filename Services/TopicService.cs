using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelEaterConsoleEdition.Services
{
    public class TopicService
    {
        public static async Task ImportTopicsFromExcelToDb(ApplicationDbContext dbContext, List<List<object>> topicNames)
        {
            // Перебираем строки Excel-данных (предполагая, что первый элемент в каждом списке - это название раздела)
            foreach (var row in topicNames)
            {
                // Получаем название раздела из первой ячейки строки
                var topicName = row.Count > 0 ? row[0]?.ToString().Trim() : null;

                if (!string.IsNullOrWhiteSpace(topicName)) // проверяем, что название не пустое
                {
                    // Проверяем, существует ли уже такой раздел в базе данных
                    var existingTopic = await dbContext.Topics.FirstOrDefaultAsync(s => s.Name == topicName);

                    if (existingTopic == null)
                    {
                        // Если такого раздела нет, добавляем новую запись
                        var newTopic = new TopicEntity
                        {
                            Name = topicName
                        };

                        dbContext.Topics.Add(newTopic);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
