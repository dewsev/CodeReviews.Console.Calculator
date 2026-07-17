using System;

namespace Calculator.dewsev;

internal static class ConsoleHelpers
{
    internal static void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
    
    internal static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop - 1;
        Console.SetCursorPosition(0, currentLineCursor);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }
}