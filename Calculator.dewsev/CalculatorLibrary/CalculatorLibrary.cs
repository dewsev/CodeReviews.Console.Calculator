namespace CalculatorLibrary;

using System;
using System.Collections.Generic;

public class Calculator
{
    public const int DefaultHistorySize = History.DefaultSize;
    
    private const string HistorySaveFileName = "history.json";
    private readonly History _history = JsonHelpers.ReadFromJsonFile<History>(HistorySaveFileName);
    
    public Operation DoOperation(OperationType operationType, double operand)
    {
        double result = operationType switch
        {
            OperationType.SquareRoot => Math.Sqrt(operand),
            OperationType.Sin => Math.Round(Math.Sin(double.DegreesToRadians(operand)), 4),
            OperationType.Tan => Math.Round(Math.Tan(double.DegreesToRadians(operand)), 4),
            OperationType.Cos => Math.Round(Math.Cos(double.DegreesToRadians(operand)), 4),
            _ => throw new InvalidOperationException("This operation will result in a mathematical error.")
        };   

        Operation operation = new Operation
        {
            Operand1 = operand,
            OperationType = operationType,
            Result = result
        };

        _history.Update(operation);

        if (_history.Size > 0)
        {
            JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);    
        }

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
            OperationType.TenToPower => Math.Pow(operand1, operand2),
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
    
    public List<Operation> GetOperationHistory()
    {
        return _history.LatestOperations;
    }
    
    public int GetTotalOperationsPerformed()
    {
        return _history.TotalOperationsPerformed;
    }
    
    public int GetHistorySize()
    {
        return _history.Size;
    }
    
    public void SetHistorySize(int newSize)
    {
        _history.SetSize(newSize);
        JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);
    }
    
    public void ClearAllData()
    {
        _history.ClearAllData();
        JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);
    }
    
    public void ClearOperationHistory()
    {
        _history.ClearOperationHistory();
        JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);
    } 
    
    public void ResetTotalOperationsPerformed()
    {
        _history.ResetTotalOperationsPerformed();
        JsonHelpers.SaveToJsonFile(_history, HistorySaveFileName);
    }
}