using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OfficeOpenXml.Utils;
using System.Threading.Tasks;

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


        public static List<List<object>> ImportSheetToList(string filePath, int sheetIndex)
        {
            ExcelPackage.License.SetNonCommercialOrganization("ABOBA");
            using var package = new ExcelPackage(new FileInfo(filePath));
            package.Compatibility.IsWorksheets1Based = true; //меняем начало индексации с 0 на 1. Убрать если полетят баги))

            if (package.File.Exists == false)
                throw new FileNotFoundException("Файл по пути: " + filePath + " не найден.");

            var worksheet = package.Workbook.Worksheets[sheetIndex];

            // Предполагаем, что первые строки являются заголовками
            var headers = new Dictionary<int, string>();
            for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                headers.Add(col, worksheet.Cells[1, col].Text);

            Console.WriteLine("Name = " + worksheet.Name);
            Console.WriteLine("Index = " + worksheet.Index);
            Console.WriteLine("MaxRow = " + worksheet.Dimension.End.Row);
            Console.WriteLine("MaxColumns = " + worksheet.Dimension.End.Column);

            // Создаем временную коллекцию данных
            var dataRows = new List<List<object>>();


            int parsedRows = 0;

            // Пропускаем первую строку-заголовок
            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var rowValues = new List<object>();

                foreach (var header in headers.Keys)
                {
                    //rowValues.Add(worksheet.Cells[row, header].Value ?? "");
                    rowValues.Add(worksheet.Cells[row, header].Value);

                }

                // Проверяем наличие хотя бы одного ненулевого значения среди элементов с индексами 2-6 включительно
                bool hasValidData = true;

                if (rowValues is null)
                {
                    hasValidData = false;
                    break;
                }


                // Если нашли хотя бы одно непустое значение, добавляем строку
                if (hasValidData)
                {
                    dataRows.Add(rowValues);
                    parsedRows++;
                }
            }
            Console.WriteLine("Parsed rows:" + parsedRows);
            return dataRows;
        }

        public static List<List<object>> ImportSheetToDatabase(string filePath, int sheetIndex)
        {
            ExcelPackage.License.SetNonCommercialOrganization("ABOBA");
            using var package = new ExcelPackage(new FileInfo(filePath));
            package.Compatibility.IsWorksheets1Based = true; //меняем начало индексации с 0 на 1. Убрать если полетят баги))
            var worksheet = package.Workbook.Worksheets[sheetIndex];

            // Предполагаем, что первые строки являются заголовками
            var headers = new Dictionary<int, string>();
            for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                headers.Add(col, worksheet.Cells[1, col].Text);

            Console.WriteLine("Name = " + worksheet.Name);
            Console.WriteLine("Index = " + worksheet.Index);
            Console.WriteLine("MaxRow = " + worksheet.Dimension.End.Row);
            Console.WriteLine("MaxColumns = " + worksheet.Dimension.End.Column);

            // Создаем временную коллекцию данных
            var dataRows = new List<List<object>>();

            var addData = new List<object>() { ReadCellValue(filePath, 1, LaunchParameters.LaunchParameters.FULL_NAME_POSITION), worksheet.Index };

            int parsedRows = 0;

            // Пропускаем первую строку-заголовок
            for (var row = 2; row <= worksheet.Dimension.End.Row; row++) //вернуть на 2
            {
                var rowValues = new List<object>();

                foreach (var header in headers.Keys)
                {
                    //rowValues.Add(worksheet.Cells[row, header].Value ?? "");
                    rowValues.Add(worksheet.Cells[row, header].Value);

                }

                // Проверяем наличие хотя бы одного ненулевого значения среди элементов с индексами 2-6 включительно
                bool hasValidData = true;
                for (int idx = 0; idx <= 4; idx++)
                {
                    if (rowValues[idx] is null)
                    {
                        hasValidData = false;
                        break;
                    }
                }

                // Если нашли хотя бы одно непустое значение, добавляем строку
                if (hasValidData)
                {
                    rowValues.InsertRange(0, addData);
                    dataRows.Add(rowValues);
                    parsedRows++;
                }
            }
            Console.WriteLine("Parsed rows:" + parsedRows);
            return dataRows;
        }
    }
}




