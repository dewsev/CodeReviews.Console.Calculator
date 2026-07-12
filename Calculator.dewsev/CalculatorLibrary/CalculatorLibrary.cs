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
    
    public double DoOperation(double num1, double num2, OperationType operationType)
    {
        double result = operationType switch
        {
            OperationType.Addition => num1 + num2,
            OperationType.Subtraction => num1 - num2,
            OperationType.Multiplication => num1 * num2,
            OperationType.Division => num1 / num2,
        };

        UpdateHistory(new Operation
        {
            Operand1 = num1,
            Operand2 = num2,
            OperationType = operationType,
            Result = result
        });

        return result;
    }

    private void UpdateHistory(Operation operation)
    {
        _history.TotalOperationsPerformed++;
        _history.LatestOperations.Add(operation);

        JsonHelpers.SaveToJsonFile(_history, HistoryFileName);
    }
}