namespace Calculator.dewsev;

using System;
using CalculatorLibrary;

internal class Program
{
    private static void Main(string[] args)
    {
        bool endApp = false;
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        Calculator calculator = new Calculator();
        while (!endApp)
        {
            OperationType operationType;
            while (true)
            {
                if (TryGetOperationTypeFromUserChoice(out operationType))
                {
                    break;
                }
                
                Console.WriteLine("Please provide a valid option.");
            }
            
            double firstOperand = GetNumberFromUser("Enter first operand: ");
            double secondOperand = GetNumberFromUser("Enter second operand: ");
           
            try
            {
                double result = calculator.DoOperation(firstOperand, secondOperand, operationType);
                if (double.IsNaN(result))
                {
                    Console.WriteLine("This operation will result in a mathematical error.\n");
                }
                else
                {
                    Console.WriteLine("Your result: {0:0.##}\n", result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }

            Console.WriteLine("------------------------\n");

            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n")
            {
                endApp = true;
            }

            Console.WriteLine("\n");
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
        
        Console.Clear();
        Console.WriteLine("Choose an operator from the following list:");
        Console.WriteLine("\t1.Add");
        Console.WriteLine("\t2.Subtract");
        Console.WriteLine("\t3.Multiply");
        Console.WriteLine("\t4.Divide");
        Console.Write("Your choice: ");

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