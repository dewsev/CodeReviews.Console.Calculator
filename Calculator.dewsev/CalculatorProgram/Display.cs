namespace Calculator.dewsev;

using System;
using System.Collections.Generic;
using CalculatorLibrary;

internal static class Display
{
    internal static void ShowOperation(Operation operation, ConsoleColor color)
    {
        try
        {
            if (operation.OperationType == OperationType.SquareRoot)
            {
                ConsoleHelpers.WriteColored($"{GetOperator(operation.OperationType)}{operation.Operand1} = {operation.Result}\n", color);
            }
            else if (operation.OperationType is OperationType.Sin or OperationType.Tan or OperationType.Cos)
            {
                ConsoleHelpers.WriteColored($"{GetOperator(operation.OperationType)}({operation.Operand1}) = {operation.Result}\n", color);
            }
            else
            {
                ConsoleHelpers.WriteColored($"{operation.Operand1} {GetOperator(operation.OperationType)} {operation.Operand2} = {operation.Result}\n", color);
            }
        }
        catch (ArgumentException ex)
        {
            ConsoleHelpers.WriteColored($"{ex.Message}\n", ConsoleColor.Red);
        }    
    }
    
    internal static void ShowTotalOperationsPerformed(Calculator calculator)
    {
        int totalOperationsPerformed = calculator.GetTotalOperationsPerformed();
        Console.WriteLine("--------------------------------------------------\n");
        ConsoleHelpers.WriteColored($"Total operations performed: {totalOperationsPerformed}\n\n", ConsoleColor.Cyan);
        Console.WriteLine("--------------------------------------------------\n");
    }
    
    internal static void ShowOperationList(List<Operation> history)
    {
        ConsoleHelpers.WriteColored("Lastest calculations:\n\n", ConsoleColor.Cyan);
        for (int i = 0; i < history.Count; i++)
        {
            ConsoleHelpers.WriteColored($"{i + 1}. ", ConsoleColor.Cyan);
            ShowOperation(history[i], ConsoleColor.White);
        }
        Console.WriteLine("\n--------------------------------------------------\n");
    }
    
    private static string GetOperator(OperationType operationType)
    {
        return operationType switch
        {
            OperationType.Addition => "+",
            OperationType.Subtraction => "-",
            OperationType.Multiplication => "*",
            OperationType.Division => "/",
            OperationType.Power => "^",
            OperationType.SquareRoot => "√",
            OperationType.Sin => "sin",
            OperationType.Tan => "tan",
            OperationType.Cos => "cos",
            OperationType.TenToPower => "^",
            _ => throw new ArgumentException("Invalid operator.")
        };
    }
}