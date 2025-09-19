using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.LaunchParameters;
using ExcelEaterConsoleEdition.Parser;
using ExcelEaterConsoleEdition.Services;
using ExcelEaterConsoleEdition.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;





class Program
{
    static async Task Main(string[] args)
    {
        using var dbContext = new ApplicationDbContext();

        await dbContext.Database.EnsureCreatedAsync();

        //List<List<object>> sectionsList = ExcelHelper.ImportSheetToList(LaunchParameters.SECTIONS_LIST_FILE_PATH, 1);
        //await SectionService.ImportSectionsFromExcelToDb(dbContext, sectionsList);

        //List<List<object>> subsectionsList = ExcelHelper.ImportSheetToList(LaunchParameters.SUBSECTIONS_LIST_FILE_PATH, 1);
        //await SubsectionService.ImportSubsectionsFromExcelToDb(dbContext, subsectionsList);

        //List<List<object>> topicsList = ExcelHelper.ImportSheetToList(LaunchParameters.TOPICS_LIST_FILE_PATH, 1);
        //await TopicService.ImportTopicsFromExcelToDb(dbContext, topicsList);

        //List<List<object>> unitsList = ExcelHelper.ImportSheetToList(LaunchParameters.UNITS_LIST_FILE_PATH, 1);
        //await UnitService.ImportUnitsFromExcelToDb(dbContext, unitsList);

        //List<List<object>> directionsList = ExcelHelper.ImportSheetToList(LaunchParameters.DIRECTIONS_LIST_FILE_PATH, 1);
        //await DirectionService.ImportDirectionsFromExcelToDb(dbContext, directionsList);

        
        

        string directoryPath = LaunchParameters.COMPETENCE_MAPS_DIRECTORY_PATH;

        try
        {
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                var parsedExcel = ExcelHelper.ImportExcelToList(file);
                await CompetencyService.ImportCompetenciesFromExcelToDb(dbContext, parsedExcel);
                ForDebugging.PrintDataRows(parsedExcel);
            }

            Console.WriteLine("Обработка завершена.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Возникла ошибка: {ex.Message}");
        }

        //await CompetencyService.ImportCompetenciesFromExcelToDb(dbContext, parsedExcel);

        //var cellValue = ExcelHelper.ReadCellValue(LaunchParameters.FILE_PATH, LaunchParameters.LEGEND_SHEET_POSITION, LaunchParameters.FULL_NAME_POSITION);
        //Console.WriteLine("ФИО:" + cellValue);

        //cellValue = ExcelHelper.ReadCellValue(LaunchParameters.FILE_PATH, LaunchParameters.LEGEND_SHEET_POSITION, LaunchParameters.UNIT_POSITION);
        //Console.WriteLine("Подразделение:" + cellValue);

        //foreach (var sheetNumber in LaunchParameters.SheetNumbersToParse)
        //{
        //    var parsedSheet = ExcelHelper.ImportSheetToList(LaunchParameters.FILE_PATH, sheetNumber);
        //    ForDebugging.PrintDataRows(parsedSheet);
        //}

     }
}