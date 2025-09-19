using ExcelEaterConsoleEdition.LaunchParameters;
using ExcelEaterConsoleEdition.Parser;
using ExcelEaterConsoleEdition.Utilities;



var cellValue = ExcelHelper.ReadCellValue(LaunchParameters.FILE_PATH, 1, 2, 5);

Console.WriteLine(cellValue);

var parsedSheet = ExcelHelper.ImportSheetToDatabase(LaunchParameters.FILE_PATH, 2);

Console.WriteLine(parsedSheet);

ForDebugging.PrintDataRows(parsedSheet);

    Console.WriteLine();