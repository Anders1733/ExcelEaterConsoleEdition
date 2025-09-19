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
        public const string FULL_NAME_POSITION = "B9";
        public const string UNIT_POSITION = "B10";
        public const int LEGEND_SHEET_POSITION = 1;
        public static readonly int[] SheetNumbersToParse = { 2, 3, 4 };
    }
}
