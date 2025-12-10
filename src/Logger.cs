namespace RetroForge.NET;

public class Logger
{
    static ConsoleColor defaultColor = ConsoleColor.Green;
    private static void Write(string msg, char end = '\n')
    {
        Console.Write($"{System.DateTime.Now}: {msg}{end}");
    }
    public static void Log(string msg, char end = '\n')
    {
        Console.ForegroundColor = defaultColor;
        Write(msg);
    }
    public static void Log(ConsoleColor _override, string msg, char end = '\n')
    {
        Console.ForegroundColor = _override;
        Write(msg);
        Console.ForegroundColor = defaultColor;
    }
    public static void Warn(string msg, char end = '\n')
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Write(msg);
        Console.ForegroundColor = defaultColor;
    }
    public static void Error(string msg, char end = '\n')
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Write(msg);
        Console.ForegroundColor = defaultColor;
    }

    public static void Assert(Func<bool> func)
    {
        if (!func())
        {
            throw new Exception($"assert on {func.ToString()} failed");
        }
    }

    public static void ResetConsole()
    {
        Console.ResetColor();
    }
}
