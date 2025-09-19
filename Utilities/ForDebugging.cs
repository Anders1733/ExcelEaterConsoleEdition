using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.Utilities
{
    public class ForDebugging
    {
        public static void PrintDataRows(List<List<object>> dataRows)
        {
            for (int i = 0; i < dataRows.Count; i++)  // Проходим по внешним спискам
            {
                Console.Write("Строка {0}: ", i + 2);  // Индексация начинается с 2, т.к. в самом файле в 1 строке заголовки

                foreach (var item in dataRows[i])      // Проходим по внутренним спискам
                {
                    if (item != null)
                        Console.Write(item.ToString() + "\t");  // Используем табуляцию для выравнивания
                    else
                        Console.Write("NULL\t");
                }

                Console.WriteLine();                    // Переход на следующую строку
            }
        }
    }

}
