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
            for (int i = 0; i < dataRows.Count; i++)
            {
                Console.Write("Строка {0}: ", i + 2);

                foreach (var item in dataRows[i]) 
                {
                    if (item != null)
                        Console.Write(item.ToString() + "\t");
                    else
                        Console.Write("NULL\t");
                }

                Console.WriteLine();
            }
        }
    }

}
