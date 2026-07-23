namespace Calculator.dewsev;

using System;
using CalculatorLibrary;
using System.Collections.Generic;

internal static class Program
{
    private static readonly Calculator Calculator = new();
    
    private static void Main(string[] args)
    {
        MenuState state = MenuState.Main;
        double? carryOperand = null;

        while (state != MenuState.Exit)
        {
            switch (state)
            {
                case MenuState.Main:
                    state = MainMenu();
                    break;
                case MenuState.Calculator:
                    state = CalculatorMenu(carryOperand);
                    break;
                case MenuState.History:
                    state = HistoryMenu(out carryOperand);
                    break;
                case MenuState.Settings:
                    state = SettingsMenu();
                    break;
            }
        }
    }

    private static MenuState CalculatorMenu(double? operand = null)
    {
        Console.Clear();
        OperationType operationType = GetOperationTypeFromUser();
        
        Console.Clear();
        
        (double operand1, double operand2) = GetOperandsFromUser(operationType, operand);

        try
        {
            Console.Clear();
            Operation operation;

            if (operationType is OperationType.SquareRoot or OperationType.Sin or OperationType.Tan
                or OperationType.Cos)
            {
                operation = Calculator.DoOperation(operationType, operand1);
            }
            else
            {
                operation = Calculator.DoOperation(operationType, operand1, operand2);
            }

            Display.ShowOperation(operation, ConsoleColor.Cyan);
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelpers.WriteColored($"{ex.Message}\n", ConsoleColor.Red);

        }
        catch (Exception ex)
        {
            ConsoleHelpers.WriteColored(
                $"Oh no! An exception occurred trying to do the math.\n - Details: {ex.Message}\n", ConsoleColor.Red);
        }

        return PostCalculationMenu();
    }
    
    private static MenuState PostCalculationMenu()
    {
        Console.WriteLine("\n1.New calculation");
        Console.WriteLine("2.History");
        Console.WriteLine("3.Main Menu");
        Console.WriteLine("4.Exit\n");

        int choice = (int)GetNumberFromUser("Your choice: ", 1, 4);

        return choice switch
        {
            1 => MenuState.Calculator,
            2 => MenuState.History,
            3 => MenuState.Main,
            4 => MenuState.Exit,
            _ => MenuState.Main
        };
    }
    
    private static MenuState MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Console Calculator in C#");
        Console.WriteLine("------------------------");
        Console.WriteLine("\n1.Calculator");
        Console.WriteLine($"2.History (Size: {Calculator.GetHistorySize()})");
        Console.WriteLine($"3.Settings");
        Console.WriteLine("4.Exit\n");

        int choice = (int)GetNumberFromUser("Your choice: ", 1, 4);

        return choice switch
        {
            1 => MenuState.Calculator,
            2 => MenuState.History,
            3 => MenuState.Settings,
            4 => MenuState.Exit,
            _ => MenuState.Main
        };
    }
    
    private static MenuState HistoryMenu(out double? selectedOperand)
    {
        selectedOperand = null;
        Console.Clear();
        
        List<Operation> operations = Calculator.GetOperationHistory();

        if (operations.Count == 0)
        {
            Console.WriteLine("Your operation history is empty.\n");
        }
        else
        {
            Display.ShowTotalOperationsPerformed(Calculator);
            Display.ShowOperationList(operations);
        }
        
        Console.WriteLine("Provide corresponding index or press ENTER to go back to Main Menu.");

        while (true)
        {
            Console.Write("Your choice: ");
            string? input = Console.ReadLine()?.ToLower().Trim();

            if (string.IsNullOrEmpty(input))
            {
                return MenuState.Main;
            }
            
            bool validChoice = int.TryParse(input, out int choice) && choice > 0 && choice <= operations.Count;
            if (validChoice)
            {
                selectedOperand = operations[choice - 1].Result;
                return MenuState.Calculator;
            }
            
            ConsoleHelpers.ClearCurrentConsoleLine();
            ConsoleHelpers.WriteColored("Invalid input. Please try again.\n", ConsoleColor.Red);
        }
    }

    private static MenuState SettingsMenu()
    {
        Console.Clear();
        Console.WriteLine("History Settings");
        Console.WriteLine("------------------------");
        Console.WriteLine("\n1.Clear all data");
        Console.WriteLine("2.Clear operation history");
        Console.WriteLine("3.Reset total operations performed");
        Console.WriteLine($"4.Change history size (Current: {Calculator.GetHistorySize()}, Default: {Calculator.DefaultHistorySize})");
        Console.WriteLine("5.Main Menu\n");

        int choice = (int)GetNumberFromUser("Your choice: ", 1, 5);

        switch (choice)
        {
            case 1:
                ClearAllData();
                break;
            case 2:
                ClearOperationHistory();
                break;
            case 3:
                ResetTotalOperationsPerformed();
                break;
            case 4:
                ChangeHistorySize();
                break;
        }

        return MenuState.Main;
    }

    private static void ChangeHistorySize()
    {
        int newSize = (int)GetNumberFromUser("Enter new history size: ", 0);
        Calculator.SetHistorySize(newSize);
    }
    
    private static void ResetTotalOperationsPerformed()
    {
        Calculator.ResetTotalOperationsPerformed();
        Console.Clear();
        ConsoleHelpers.WriteColored("Total operations performed reset.\n", ConsoleColor.Green);
        Console.WriteLine("Press any key to go back to Main Menu.");
        Console.ReadKey();
    }
    
    private static void ClearOperationHistory()
    {
        Calculator.ClearOperationHistory();
        Console.Clear();
        ConsoleHelpers.WriteColored("Operation history cleared.\n", ConsoleColor.Green);
        Console.WriteLine("Press any key to go back to Main Menu.");
        Console.ReadKey();
    }
    
    private static void ClearAllData()
    {
        Calculator.ClearAllData();
        Console.Clear();
        ConsoleHelpers.WriteColored("Data cleared.\n", ConsoleColor.Green);
        Console.WriteLine("Press any key to go back to Main Menu.");
        Console.ReadKey();
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
            Console.WriteLine(FormatOperationName(name));
        }
        Console.WriteLine();

        int choice = (int)GetNumberFromUser("Your choice: ", 1, operationTypeNames.Length) - 1;
        return (OperationType)choice;
    }

    private static string FormatOperationName(string name)
    {
        string newName = name[0].ToString();

        for (int i = 1; i < name.Length; i++)
        {
            string letter = name[i].ToString();
            newName += letter == letter.ToUpper() ? $" {letter.ToLower()}" : letter; 
        }

        return newName;
    }
}



