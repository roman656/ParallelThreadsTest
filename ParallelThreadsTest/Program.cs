namespace ParallelThreadsTest;

using System.Diagnostics;

public static class Program
{
    private const int NumbersAmount = 1000000;
    private const string FileDirectoryName = "Results";
    private static readonly int ThreadsAmount = Environment.ProcessorCount;
    
    public static void Main(string[] args)
    {
        var threads = new Thread[ThreadsAmount];

        Console.WriteLine("Количество доступных процессоров (логических): {0}", ThreadsAmount);
        DisplayTimerProperties();
        CheckDirectory(FileDirectoryName);

        for (var i = 0; i < ThreadsAmount; i++)
        {
            threads[i] = new Thread(Task);
            threads[i].Start(i);
        }
        
        for (var j = 0; j < ThreadsAmount; j++)
        {
            threads[j].Join();
        }
        
        Console.WriteLine("Все потоки отработали успешно");
    }
    
    private static void Task(object? index)
    {
        var random = new Random();
        var path = FileDirectoryName + "/" + index + ".txt";
        var timeForOperations = Stopwatch.StartNew();
        
        for (var i = 0; i < NumbersAmount; i++)
        {
            random.Next();
        }
        
        timeForOperations.Stop();
        
        using (var writer = new StreamWriter(path, false))
        {
            writer.WriteLine($"Затрачено времени (мс): {timeForOperations.ElapsedMilliseconds}");
        }
        
        Console.WriteLine("Поток {0:d2} -> затрачено времени (мс): {1}", index, timeForOperations.ElapsedMilliseconds);
    }
    
    private static void CheckDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    
    private static void DisplayTimerProperties()
    {
        var frequency = Stopwatch.Frequency;
        var nanosecondsPerTick = 1_000_000_000L / frequency;
        
        Console.WriteLine("Параметры таймера:");
        Console.WriteLine(Stopwatch.IsHighResolution
                ? "- Используется системный счётчик производительности с высоким разрешением"
                : "- Используется класс DateTime");
        Console.WriteLine("- Частота таймера (тактов в секунду): {0}", frequency);
        Console.WriteLine("- Таймер работает с точностью до {0} нс", nanosecondsPerTick);
    }
}