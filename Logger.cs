namespace RetroForge.NET;

public class Logger
{
    static ConsoleColor defaultColor = ConsoleColor.Green;
    private static void Write(string msg)
    {
        Console.WriteLine($"{System.DateTime.Now}: {msg}");
    }
    public static void Log(string msg)
    {
        Console.ForegroundColor = defaultColor;
        Write(msg);
    }
    public static void Log(ConsoleColor _override, string msg)
    {
        Console.ForegroundColor = _override;
        Write(msg);
        Console.ForegroundColor = defaultColor;
    }
    public static void Warn(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Write(msg);
        Console.ForegroundColor = defaultColor;
    }
    public static void Error(string msg)
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
