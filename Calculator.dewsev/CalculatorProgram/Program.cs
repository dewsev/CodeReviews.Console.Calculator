using System.Collections.Generic;

namespace Calculator.dewsev;

using System;
using CalculatorLibrary;

internal class Program
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
        double operand1 = operand ?? GetNumberFromUser("Enter first operand: ");
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
        Console.WriteLine("4.Exit");
        Console.Write("\nYour choice: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CalculatorMenu();
                break;
            case "2":
                HistoryMenu();
                break;
            case "3":
                MainMenu();
                break;
            case "4":
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
        
        List<Operation> history = Calculator.GetOperationHistory();

        if (history.Count == 0)
        {
            Console.WriteLine("You have not performed any calculations yet.");
            Console.WriteLine("Press any key to go to main menu.");
            Console.ReadKey();
            MainMenu();
        }
        else
        {
            DisplayCalculationHistory(history);
            
            Console.WriteLine("\nProvide corresponding index or press ENTER to go back to main menu.");
            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    MainMenu();
                }
                else
                {
                    bool validChoice = int.TryParse(input, out int choice) && choice > 0 && choice <= history.Count;
                    if (validChoice)
                    {
                        double chosenResult = history[choice - 1].Result;
                        CalculatorMenu(chosenResult);
                    }
                    WriteColored("Invalid input. Please try again.\n", ConsoleColor.Red);    
                }
            }
        }
    }

    private static void DisplayCalculationHistory(List<Operation> history)
    {
        Console.WriteLine("Lastest calculations:\n");
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
    }
    
    private static void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Console Calculator in C#");
        Console.WriteLine("------------------------");
        Console.WriteLine("\n1.Calculator");
        Console.WriteLine("2.History");
        Console.WriteLine("3.Exit");
        Console.Write("\nYour choice: ");
        
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CalculatorMenu();
                break;
            case "2":
                HistoryMenu();
                break;
            case "3":
                Environment.Exit(0);
                break;
        }
    }
    
    private static double GetNumberFromUser(string prompt)
    {
        Console.Clear();
        Console.Write(prompt);
        string? numInput1 = Console.ReadLine();

        double cleanNum1;
        while (!double.TryParse(numInput1, out cleanNum1))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput1 = Console.ReadLine();
        }

        return cleanNum1;
    }

    private static OperationType GetOperationTypeFromUser()
    {
        Console.WriteLine("Choose an operation:");
        Console.WriteLine("\n1.Add");
        Console.WriteLine("2.Subtract");
        Console.WriteLine("3.Multiply");
        Console.WriteLine("4.Divide\n");
        
        while (true)
        {
            Console.Write("Your choice: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1": 
                    return OperationType.Addition;
                case "2": 
                    return OperationType.Subtraction;
                case "3":
                    return OperationType.Multiplication;
                case "4": 
                    return OperationType.Division;
                default:
                    WriteColored("Please provide a valid option.\n", ConsoleColor.Cyan);
                    break;
            }
        }
    }
}