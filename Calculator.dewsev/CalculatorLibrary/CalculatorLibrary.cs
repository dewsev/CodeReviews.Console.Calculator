using System;
using System.Collections.Generic;

namespace CalculatorLibrary;

public class Calculator
{
    private const string HistoryFileName = "history.json";
    private readonly History _history = JsonHelpers.ReadFromJsonFile<History>(HistoryFileName);

    public List<Operation> GetOperationHistory()
    {
        return _history.LatestOperations;
    }
    
    public Operation DoOperation(double operand1, double operand2, OperationType operationType)
    {
        double result = operationType switch
        {
            OperationType.Addition => operand1 + operand2,
            OperationType.Subtraction => operand1 - operand2,
            OperationType.Multiplication => operand1 * operand2,
            OperationType.Division => operand2 == 0 ? double.NaN : operand1 / operand2,
            _ => double.NaN
        };

        if (double.IsNaN(result))
        {
            throw new InvalidOperationException("This operation will result in a mathematical error.");
        }
        
        Operation operation = new Operation
        {
            Operand1 = operand1,
            Operand2 = operand2,
            OperationType = operationType,
            Result = result
        };
        
        UpdateHistory(operation);

        return operation;
    }

    private void UpdateHistory(Operation operation)
    {
        _history.TotalOperationsPerformed++;
        _history.LatestOperations.Add(operation);

        JsonHelpers.SaveToJsonFile(_history, HistoryFileName);
    }
}