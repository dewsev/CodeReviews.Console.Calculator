namespace CalculatorLibrary;

using System;
using System.Collections.Generic;

public class Calculator
{
    private const string HistorySaveFileName = "history.json";
    private readonly History _history = JsonHelpers.ReadFromJsonFile<History>(HistorySaveFileName);

    public List<Operation> GetOperationHistory()
    {
        return _history.LatestOperations;
    }
    
    public int GetTotalOperationsPerformed()
    {
        return _history.TotalOperationsPerformed;
    }
    
    public Operation DoOperation(OperationType operationType, double operand)
    {
        double result = operationType switch
        {
            OperationType.SquareRoot => Math.Sqrt(operand),
            OperationType.Sin => Math.Round(Math.Sin(double.DegreesToRadians(operand)), 4),
            OperationType.Tan => Math.Round(Math.Tan(double.DegreesToRadians(operand)), 4),
            _ => throw new InvalidOperationException("This operation will result in a mathematical error.")
        };   

        Operation operation = new Operation
        {
            Operand1 = operand,
            OperationType = operationType,
            Result = result
        };

        _history.Update(operation);
        JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);

        return operation;
    }
    
    public Operation DoOperation(OperationType operationType, double operand1, double operand2)
    {
        double result = operationType switch
        {
            OperationType.Addition => operand1 + operand2,
            OperationType.Subtraction => operand1 - operand2,
            OperationType.Multiplication => operand1 * operand2,
            OperationType.Division => operand2 == 0 ? double.NaN : operand1 / operand2,
            OperationType.Power => Math.Pow(operand1, operand2),
            _ => throw new InvalidOperationException("This operation will result in a mathematical error.")
        };   

        Operation operation = new Operation
        {
            Operand1 = operand1,
            Operand2 = operand2,
            OperationType = operationType,
            Result = result
        };

        _history.Update(operation);
        JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);

        return operation;
    }

    public void ClearHistory()
    {
        _history.Clear();
        JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);
    }
}