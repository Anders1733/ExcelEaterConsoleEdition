using ExcelEaterConsoleEdition.Parser;

const string FILE_PATH = "C:\\Projects\\ExcelEaterConsoleEdition\\PlateForExcelFiles\\test.xlsl";

var cellValue = Parser.ReadCellValue("test.xlsx", 0, 2, 5);

Console.WriteLine(cellValue);
