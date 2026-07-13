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

    private static void CalculatorMenu()
    {
        Console.Clear();
        
        OperationType operationType;
        while (true)
        {
            if (TryGetOperationTypeFromUserChoice(out operationType))
            {
                Console.Clear();
                break;
            }
                
            Console.WriteLine("Please provide a valid option.");
        }
            
        double firstOperand = GetNumberFromUser("Enter first operand: ");
        double secondOperand = GetNumberFromUser("Enter second operand: ");
           
        try
        {
            double result = Calculator.DoOperation(firstOperand, secondOperand, operationType);
            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation will result in a mathematical error.\n");
            }
            else
            {
                Console.WriteLine($"Your result: {result:N2}");
                
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
        }
        catch (Exception e)
        {
            Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
        }
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
            Console.WriteLine("Lastest calculations:\n");
            for (int i = 0; i < history.Count; i++)
            {
                Operation operation = history[i];
                char op;
                try
                {
                    op = operation.OperationType switch
                    {
                        OperationType.Addition => '+',
                        OperationType.Subtraction => '-',
                        OperationType.Multiplication => '*',
                        OperationType.Division => '/',
                        _ => throw new InvalidOperationException("Invalid operation.")
                    };
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
            
                Console.WriteLine($"{i + 1}. {operation.Operand1} {op} {operation.Operand2} = {operation.Result}");
            }
            
            Console.WriteLine("Enter calculation number or 'm' to go to Main Menu.");
            while (true)
            {
                string? input = Console.ReadLine();

                if (input == "m")
                {
                    MainMenu();
                }
                else
                {
                    bool validChoice = int.TryParse(input, out int choice) && choice > 0 && choice <= history.Count;
                    if (validChoice)
                    {
                        // Proceed to CalculatorMenu with the operation's result as the first operand
                        double chosenResult = history[choice - 1].Result;
                        CalculatorMenu();
                    }
                    Console.WriteLine("Invalid input. Please try again.");    
                }
            }
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

    private static bool TryGetOperationTypeFromUserChoice(out OperationType operationType)
    {
        operationType = default;
        
        Console.WriteLine("Choose an operation:");
        Console.WriteLine("\n1.Add");
        Console.WriteLine("2.Subtract");
        Console.WriteLine("3.Multiply");
        Console.WriteLine("4.Divide");
        Console.Write("\nYour choice: ");

        string? choice = Console.ReadLine();

         switch (choice)
        {
            case "1": operationType = OperationType.Addition; return true;
            case "2": operationType = OperationType.Subtraction; return true;
            case "3": operationType = OperationType.Multiplication; return true;
            case "4": operationType = OperationType.Division; return true;
        }

        return false;
    }
}