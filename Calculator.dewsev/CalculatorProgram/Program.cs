namespace Calculator.dewsev;

using System;
using CalculatorLibrary;
using System.Collections.Generic;

internal static class Program
{
    private static readonly Calculator Calculator = new Calculator();
    
    private static void Main(string[] args)
    {
        MainMenu();
    }

    private static void CalculatorMenu(double? operand = null)
    {
        Console.Clear();
        OperationType operationType = GetOperationTypeFromUser();
        
        Console.Clear();
        double operand1 = operand ?? GetNumberFromUser("Enter first operand: ");
        
        Console.Clear();
        double operand2 = GetNumberFromUser("Enter second operand: ");

        try
        {
            Operation operation = Calculator.DoOperation(operand1, operand2, operationType);

            Console.Clear();
            DisplayOperation(operation);
            PostCalculationMenu();
        }
        catch (InvalidOperationException ex)
        {
            Console.Clear();
            WriteColored($"{ex.Message}\n", ConsoleColor.Red);
            PostCalculationMenu();
        }
        catch (Exception ex)
        {
            WriteColored($"Oh no! An exception occurred trying to do the math.\n - Details: {ex.Message}\n", ConsoleColor.Red);
        }
    }

    private static void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
    
    private static void PostCalculationMenu()
    {
        Console.WriteLine("\n1.New calculation");
        Console.WriteLine("2.History");
        Console.WriteLine("3.Main Menu");
        Console.WriteLine("4.Exit\n");

        int choice = (int)GetNumberFromUser("Your choice: ", 1, 4);
            
        switch (choice)
        {
            case 1:
                CalculatorMenu();
                break;
            case 2:
                HistoryMenu();
                break;
            case 3:
                MainMenu();
                break;
            case 4:
                Environment.Exit(0);
                break;
        }
    }
    
    private static void DisplayOperation(Operation operation)
    {
        try
        {
            WriteColored($"{operation.Operand1} {GetOperator(operation.OperationType)} {operation.Operand2} = {operation.Result}\n", ConsoleColor.Cyan);
        }
        catch (ArgumentException ex)
        {
            WriteColored($"{ex.Message}\n", ConsoleColor.Red);
        }
    }


    private static char GetOperator(OperationType operationType)
    {
        return operationType switch
        {
            OperationType.Addition => '+',
            OperationType.Subtraction => '-',
            OperationType.Multiplication => '*',
            OperationType.Division => '/',
            _ => throw new ArgumentException("Invalid operator.")
        };
    }
    
    private static void HistoryMenu()
    {
        Console.Clear();
        
        List<Operation> operations = Calculator.GetOperationHistory();

        if (operations.Count == 0)
        {
            Console.WriteLine("You have not performed any calculations yet.\n");
            Console.WriteLine("Press any key to go to Main Menu.");
            Console.ReadKey();
            MainMenu();
        }
        else
        {
            DisplayTotalOperationsPerformed();
            DisplayOperationList(operations);
            
            Console.WriteLine("Provide corresponding index or press ENTER to go back to Main Menu.");
            Console.WriteLine("Input 'c' and press ENTER to clear the history data.\n");

            while (true)
            {
                Console.Write("Your choice: ");
                string? input = Console.ReadLine()?.ToLower().Trim();

                if (string.IsNullOrEmpty(input))
                {
                    MainMenu();
                }
                else if (input == "c")
                {
                    ClearHistory();
                }
                else
                {
                    bool validChoice = int.TryParse(input, out int choice) && choice > 0 && choice <= operations.Count;
                    if (validChoice)
                    {
                        double chosenResult = operations[choice - 1].Result;
                        CalculatorMenu(chosenResult);
                    }
                    
                    ClearCurrentConsoleLine();
                    WriteColored("Invalid input. Please try again.\n", ConsoleColor.Red);
                }
            }
        }
    }

    private static void ClearHistory()
    {
        Calculator.ClearHistory();
        Console.Clear();
        WriteColored("History cleared.\n", ConsoleColor.Green);
        Console.WriteLine("Press any key to go back to main menu.");
        Console.ReadKey();
        MainMenu();
    }

    private static void DisplayTotalOperationsPerformed()
    {
        int totalOperationsPerformed = Calculator.GetTotalOperationsPerformed();
        Console.WriteLine("--------------------------------------------------\n");
        WriteColored($"Total operations performed: {totalOperationsPerformed}\n\n", ConsoleColor.Cyan);
        Console.WriteLine("--------------------------------------------------\n");
    }
    
    private static void DisplayOperationList(List<Operation> history)
    {
        WriteColored("Lastest calculations:\n\n", ConsoleColor.Cyan);
        for (int i = 0; i < history.Count; i++)
        {
            Operation operation = history[i];
            
            char op;
            try
            {
                op = GetOperator(operation.OperationType);
            }
            catch (ArgumentException)
            {
                continue;
            }

            WriteColored($"{i + 1}. ", ConsoleColor.Cyan);
            Console.Write($"{operation.Operand1} {op} {operation.Operand2} = {operation.Result}\n");
        }
        Console.WriteLine("\n--------------------------------------------------\n");
    }
    
    private static void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Console Calculator in C#");
        Console.WriteLine("------------------------");
        Console.WriteLine("\n1.Calculator");
        Console.WriteLine("2.History");
        Console.WriteLine("3.Exit\n");

        int choice = (int)GetNumberFromUser("Your choice: ", 1, 3);
            
        switch (choice)
        {
            case 1:
                CalculatorMenu();
                break;
            case 2:
                HistoryMenu();
                break;
            case 3:
                Environment.Exit(0);
                break;
        }
    }
    
    private static double GetNumberFromUser(string? prompt, double min = double.MinValue, double max = double.MaxValue)
    {
        string? input;
        double cleanNum;
        do
        {
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.Write(prompt);
            }
            
            input = Console.ReadLine();
            
            ClearCurrentConsoleLine();

        } while (!double.TryParse(input, out cleanNum) || cleanNum < min || cleanNum > max);

        return cleanNum;
    }

    private static OperationType GetOperationTypeFromUser()
    {
        Console.WriteLine("Choose an operation:");
        Console.WriteLine("\n1.Add");
        Console.WriteLine("2.Subtract");
        Console.WriteLine("3.Multiply");
        Console.WriteLine("4.Divide\n");
        
        int choice = (int)GetNumberFromUser("Your choice: ", 1, 4);

        return choice switch
        {
            1 => OperationType.Addition,
            2 => OperationType.Subtraction,
            3 => OperationType.Multiplication,
            4 => OperationType.Division,
            _ => throw new ArgumentException("Invalid input provided.")
        };
    }
    
    private static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop - 1;
        Console.SetCursorPosition(0, currentLineCursor);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }
}

