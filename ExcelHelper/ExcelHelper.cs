using ExcelEaterConsoleEdition.Utilities;
using OfficeOpenXml;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ExcelEaterConsoleEdition.Parser
{
    public class ExcelHelper
    {
        ExcelHelper()
        {
            ExcelPackage.License.SetNonCommercialOrganization("ABOBA");
        }

        public static string ReadCellValue(string filePath, int sheetIndex, string cellAdress)
        {
            ExcelPackage.License.SetNonCommercialOrganization("ABOBA");


            var rowNumber = 0;

            var columnNumber = 0;
            var R1C1CellAdress = ExcelCellBase.TranslateToR1C1(cellAdress, 0, 0);



            Regex regex = new Regex(@"R(\[-?\d+\])C(\[-?\d+\])|R(\[-?\d+\])C|RC(\[-?\d+\])");

            Match match = regex.Match(R1C1CellAdress);

            rowNumber = int.Parse(match.Groups[1].Value.Replace("[", "").Replace("]", ""));
            columnNumber = int.Parse(match.Groups[2].Value.Replace("[", "").Replace("]", ""));
            using var package = new ExcelPackage(new FileInfo(filePath));

            package.Compatibility.IsWorksheets1Based = true; //меняем начало индексации с 0 на 1. Убрать если полетят баги))

            if (package.File.Exists == false)
                throw new FileNotFoundException("Файл не найден.");

            var worksheet = package.Workbook.Worksheets[sheetIndex];

            if (worksheet.Dimension.End.Row >= rowNumber && worksheet.Dimension.End.Column >= columnNumber)
            {
                return worksheet.Cells[rowNumber, columnNumber].Text;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Указанная ячейка выходит за пределы диапазона листа.");
            }
        }

        public static List<List<object>> ImportSingleSheetToList(string filePath, int sheetIndex)
        {
            
            ExcelPackage.License.SetNonCommercialOrganization("ABOBA");

            

            using var package = new ExcelPackage(new FileInfo(filePath));
            package.Compatibility.IsWorksheets1Based = true; //меняем начало индексации листов с 0 на 1. Убрать если полетят баги))

            

            if (package.File.Exists == false)
                throw new FileNotFoundException("Файл по пути: " + filePath + " не найден.");

            var worksheet = package.Workbook.Worksheets[sheetIndex];

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var headers = new Dictionary<int, string>();
            for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                headers.Add(col, worksheet.Cells[1, col].Text);

            var dataRows = new List<List<object>>();


            int parsedRows = 0;

            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var rowValues = new List<object>();

                foreach (var columnNumber in headers.Keys)
                {
                    object cellValue = worksheet.Cells[row, columnNumber].Value;

                    if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                        rowValues.Add(cellValue);   
                }

                if (rowValues.Count > 0)
                {
                    dataRows.Add(rowValues);
                    parsedRows++;
                }
            }

            TimeSpan elapsedTime = stopwatch.Elapsed;
            Logger.Performance($"Время обработки страницы {worksheet.Name} : {elapsedTime.TotalMilliseconds} мс");
            return dataRows;
        }

        public static string GetSheetNameBySheetIndex(string filePath, int sheetIndex)
        {
            ExcelPackage.License.SetNonCommercialOrganization("ABOBA");
            var package = new ExcelPackage(new FileInfo(filePath));
            package.Compatibility.IsWorksheets1Based = true; //меняем начало индексации с 0 на 1. Убрать если полетят баги))
            return package.Workbook.Worksheets[sheetIndex].Name;
        }
    }
}




