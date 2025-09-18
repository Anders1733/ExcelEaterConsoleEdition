using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace ExcelEaterConsoleEdition.Parser
{
    public class Parser
    {
        public static string ReadCellValue(string filePath, int sheetIndex, int rowNumber, int columnNumber)
        {
            ExcelPackage.License.SetNonCommercialOrganization("ABOBA");
            using var package = new ExcelPackage(new FileInfo(filePath));

            Console.WriteLine(package.Workbook.Worksheets.Count);
            

            var worksheet = package.Workbook.Worksheets[sheetIndex];
            Console.WriteLine();
            foreach (var worksheet1 in package.Workbook.Worksheets)
            {
                Console.WriteLine("Name = " + worksheet1.Name);
                Console.WriteLine("Index = " + worksheet1.Index);
                Console.WriteLine("MaxRow = " + worksheet.Dimension.End.Row);
                Console.WriteLine("MaxColumns = " + worksheet.Dimension.End.Column);
            }
            
            // Проверяем наличие строки и столбца перед чтением
            if (worksheet.Dimension.End.Row >= rowNumber && worksheet.Dimension.End.Column >= columnNumber)
            {
                return worksheet.Cells[rowNumber, columnNumber].Text;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Указанная ячейка выходит за пределы диапазона листа.");
            }
        }
    }

}
