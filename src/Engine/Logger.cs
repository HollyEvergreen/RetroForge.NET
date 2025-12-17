using System.Collections;
using System.Text;

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
        Write(msg, end);
    }
    public static void Log(ConsoleColor _override, string msg, char end = '\n')
    {
        Console.ForegroundColor = _override;
        Write(msg, end);
        Console.ForegroundColor = defaultColor;
    }
    public static void Warn(string msg, char end = '\n')
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Write(msg, end);
        Console.ForegroundColor = defaultColor;
    }
    public static void Error(string msg, char end = '\n')
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Write(msg, end);
        Console.ForegroundColor = defaultColor;
    }
    public static void Error(Exception err)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{err.GetType()} => {err.Source}\n{err.Message}");
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

    public static void LogArray<T>(T[] array, string name = "array")
    {
        StringBuilder msg = new(array.Length * 32);
        int i = 0;
        msg.Append($"{array} = [\n\t");
        foreach (T element in array)
        {
            msg.Append($"{element}, ");
            i++;
            i %= 16;
            if (i == 0) msg.Append("\n\t");
        }
        Log(msg.ToString());
    }
}
