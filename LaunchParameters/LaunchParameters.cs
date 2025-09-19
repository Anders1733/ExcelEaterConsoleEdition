using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.LaunchParameters
{
    public class LaunchParameters
    {
        public const string FILE_PATH = "C:\\Projects\\ExcelEaterConsoleEdition\\PlateForExcelFiles\\test1.xlsx";

        public const string COMETENCE_MAPS_DIRECTORY_PATH = "C:\\Projects\\ExcelEaterConsoleEdition\\PlateForExcelFiles\\Competence maps";

        public const string SECTIONS_LIST_FILE_PATH = "C:\\Projects\\ExcelEaterConsoleEdition\\PlateForExcelFiles\\SectionsList.xlsx";
        public const string SUBSECTIONS_LIST_FILE_PATH = "C:\\Projects\\ExcelEaterConsoleEdition\\PlateForExcelFiles\\SubsectionsList.xlsx";
        public const string TOPICS_LIST_FILE_PATH = "C:\\Projects\\ExcelEaterConsoleEdition\\PlateForExcelFiles\\TopicsList.xlsx";
        public const string UNITS_LIST_FILE_PATH = "C:\\Projects\\ExcelEaterConsoleEdition\\PlateForExcelFiles\\UnitsList.xlsx";

        public const string FULL_NAME_POSITION = "B9";
        public const string UNIT_POSITION = "B10";
        public const int LEGEND_SHEET_POSITION = 1;
        public static readonly int[] SheetNumbersToParse = { 2, 3, 4 };

        public const string DB_CONNECTION_STRING = "Server=WIN-57D35B7TG62\\SQLEXPRESS; DataBase=ParsedExcel; TrustServerCertificate=True; Trusted_Connection=True; MultipleActiveResultSets=true";
    }
}
