using ExcelEaterConsoleEdition.LaunchParameters;
using ExcelEaterConsoleEdition.Parser;
using ExcelEaterConsoleEdition.Utilities;



var cellValue = ExcelHelper.ReadCellValue(LaunchParameters.FILE_PATH, LaunchParameters.LEGEND_SHEET_POSITION, LaunchParameters.FULL_NAME_POSITION);
Console.WriteLine("ФИО:" + cellValue);

cellValue = ExcelHelper.ReadCellValue(LaunchParameters.FILE_PATH, LaunchParameters.LEGEND_SHEET_POSITION, LaunchParameters.UNIT_POSITION);
Console.WriteLine("Подразделение:" + cellValue);

var parsedSheet = ExcelHelper.ImportSheetToDatabase(LaunchParameters.FILE_PATH, 2);

Console.WriteLine(parsedSheet);

ForDebugging.PrintDataRows(parsedSheet);

    Console.WriteLine();