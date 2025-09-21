using ExcelEaterConsoleEdition.Database;
using ExcelEaterConsoleEdition.LaunchParameters;
using ExcelEaterConsoleEdition.Services;
using ExcelEaterConsoleEdition.Utilities;
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

        var filesCount = 0;

        try
        {
            
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                
                stopwatch.Reset();
                stopwatch.Start();
                filesCount++;

                await CompetencyService.ImportCompetenciesFromExcelToDb(dbContext, file);
                stopwatch.Stop();

                TimeSpan elapsedTime = stopwatch.Elapsed;
                Logger.Performance($"Время обработки {filesCount} файла {file} : {elapsedTime.TotalMilliseconds} мс");
                Logger.Info($"Файлов обработано: {filesCount}");

            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Возникла ошибка: {ex.Message}");
        }
        Logger.Info($"Файлов обработано: {filesCount}");
        programStopwatch.Stop();
        TimeSpan programElapsedTime = programStopwatch.Elapsed;
        Logger.Performance($"Среднее время обработки одного файла: {programElapsedTime.TotalMilliseconds/filesCount} мс");
        Logger.Info("Завершение работы программы.");
        
        
        Logger.Performance($"Время выполнения программы: {programElapsedTime.TotalMilliseconds} мс");
    }
}