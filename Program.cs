using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.LaunchParameters;
using ExcelEaterConsoleEdition.Parser;
using ExcelEaterConsoleEdition.Services;
using ExcelEaterConsoleEdition.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;





class Program
{
    static async Task Main(string[] args)
    {
        Logger.IsLoggingEnabled = true;
        Logger.IsPerformanceLoggingEnabled = true;

        var programStopwatch = new Stopwatch();
        programStopwatch.Start();
        Logger.Info("Начало работы программы.");

        

        using var dbContext = new ApplicationDbContext();

        await dbContext.Database.EnsureCreatedAsync();

        
        

        string directoryPath = LaunchParameters.COMPETENCE_MAPS_DIRECTORY_PATH;

        //замер производительности
        var stopwatch = new Stopwatch();



        try
        {
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                stopwatch.Reset();
                stopwatch.Start();
                Logger.Info($"Начало обработки файла = {file}");
                //var parsedExcel = ExcelHelper.ImportExcelToList(file);
                await CompetencyService.ImportCompetenciesFromExcelToDb(dbContext, file);
                stopwatch.Stop();
                Logger.Info($"Конец обработки файла = {file}");
                TimeSpan elapsedTime = stopwatch.Elapsed;
                Logger.Performance($"Время выполнения: {elapsedTime.TotalMilliseconds} мс");
            }

            Console.WriteLine("Обработка завершена.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Возникла ошибка: {ex.Message}");
        }

        programStopwatch.Stop();
        Logger.Info("Завершение работы программы.");
        
        TimeSpan programElapsedTime = programStopwatch.Elapsed;
        Logger.Performance($"Время выполнения программы: {programElapsedTime.TotalMilliseconds} мс");
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