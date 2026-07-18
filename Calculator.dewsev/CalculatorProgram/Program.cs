namespace Calculator.dewsev;

using System;
using CalculatorLibrary;
using System.Collections.Generic;

internal static class Program
{
    private static readonly Calculator Calculator = new();
    
    private static void Main(string[] args)
    {
        MainMenu();
    }

    private static void CalculatorMenu(double? operand = null)
    {
        Console.Clear();
        OperationType operationType = GetOperationTypeFromUser();
        
        Console.Clear();
        
        (double operand1, double operand2) = GetOperandsFromUser(operationType, operand);

        try
        {
            Operation operation;
            
            if (operationType is OperationType.SquareRoot or OperationType.Sin or OperationType.Tan or OperationType.Cos)
            {
                operation = Calculator.DoOperation(operationType, operand1);
            }
            else
            {
                operation = Calculator.DoOperation(operationType, operand1, operand2);    
            }
            
            Console.Clear();
            Display.ShowOperation(operation, ConsoleColor.Cyan);
            PostCalculationMenu();
        }
        catch (InvalidOperationException ex)
        {
            Console.Clear();
            ConsoleHelpers.WriteColored($"{ex.Message}\n", ConsoleColor.Red);
            PostCalculationMenu();
        }
        catch (Exception ex)
        {
            ConsoleHelpers.WriteColored($"Oh no! An exception occurred trying to do the math.\n - Details: {ex.Message}\n", ConsoleColor.Red);
        }
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
            Display.ShowTotalOperationsPerformed(Calculator);
            Display.ShowOperationList(operations);
            
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
                    
                    ConsoleHelpers.ClearCurrentConsoleLine();
                    ConsoleHelpers.WriteColored("Invalid input. Please try again.\n", ConsoleColor.Red);
                }
            }
        }
    }
    
    private static void ClearHistory()
    {
        Calculator.ClearHistory();
        Console.Clear();
        ConsoleHelpers.WriteColored("History cleared.\n", ConsoleColor.Green);
        Console.WriteLine("Press any key to go back to main menu.");
        Console.ReadKey();
        MainMenu();
    }

    private static (double, double) GetOperandsFromUser(OperationType operationType, double? operand = null)
    {
        double operand1;
        double operand2;

        if (operationType == OperationType.SquareRoot)
        {
            operand1 = operand ?? GetNumberFromUser("Radicand: ");
            operand2 = double.NaN;
        }
        else if (operationType is OperationType.Sin or OperationType.Tan or OperationType.Cos)
        {
            operand1 = operand ?? GetNumberFromUser("Angle in degrees: ");
            operand2 = double.NaN;
        }
        else if (operationType == OperationType.TenToPower)
        {
            operand1 = 10;
            operand2 = (int)GetNumberFromUser("Power: ");
        }
        else if (operationType == OperationType.Power)
        {
            Console.Clear();
            operand1 = operand ?? GetNumberFromUser("Base number: ");
            
            Console.Clear();
            operand2 = (int)GetNumberFromUser("Power: ");
        }
        else
        {
            Console.Clear();
            operand1 = operand ?? GetNumberFromUser("First operand: ");
            
            Console.Clear();
            operand2 = GetNumberFromUser("Second operand: ");
        }

        return (operand1, operand2);
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
            
            ConsoleHelpers.ClearCurrentConsoleLine();

        } while (!double.TryParse(input, out cleanNum) || cleanNum < min || cleanNum > max);

        return cleanNum;
    }

    private static OperationType GetOperationTypeFromUser()
    {
        Console.WriteLine("Choose an operation:");

        string[] operationTypeNames = Enum.GetNames<OperationType>();
        
        Console.WriteLine();
        for (int i = 0; i < operationTypeNames.Length; i++)
        {
            string name = operationTypeNames[i];

            Console.Write($"{i + 1}.");
            if (name == "SquareRoot")
            {
                name = "Square Root";
            }
            else if (name == "TenToPower")
            {
                name = "Ten to power";
            }

            Console.WriteLine(name);
        }
        Console.WriteLine();

        int choice = (int)GetNumberFromUser("Your choice: ", 1, operationTypeNames.Length) - 1;
        return (OperationType)choice;
    }
}

