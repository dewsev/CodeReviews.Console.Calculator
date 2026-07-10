namespace CalculatorLibrary;

public class Calculator
{
    private const string HistoryFileName = "history.json";
    private readonly History _history = JsonHelpers.ReadFromJsonFile<History>(HistoryFileName);

    // TODO: This needs to change
    public double DoOperation(double num1, double num2, string op)
    {
        var result = double.NaN;

        switch (op)
        {
            case "a":
                result = num1 + num2;
                break;
            case "s":
                result = num1 - num2;
                break;
            case "m":
                result = num1 * num2;
                break;
            case "d":
                if (num2 != 0) result = num1 / num2;

                break;
        }

        UpdateHistory(new Operation
        {
            Operand1 = num1,
            Operand2 = num2,
            OperationType = GetOperationType(op),
            Result = result
        });

        return result;
    }

    // TODO: This needs to change
    private OperationType GetOperationType(string operation)
    {
        return operation switch
        {
            "a" => OperationType.Addition,
            "s" => OperationType.Subtraction,
            "m" => OperationType.Multiplication,
            "d" => OperationType.Division
        };
    }

    private void UpdateHistory(Operation operation)
    {
        _history.TotalOperationsPerformed++;
        _history.LatestOperations.Add(operation);

        JsonHelpers.SaveToJsonFile(_history, HistoryFileName);
    }
}