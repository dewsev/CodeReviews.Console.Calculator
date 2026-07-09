using System.Collections.Generic;

namespace CalculatorLibrary;

using System.IO;
using Newtonsoft.Json;

public class Calculator
{
    private readonly List<Operation> _operations = []; 
    
    public double DoOperation(double num1, double num2, string op)
    {
        double result = double.NaN;
        
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
                if (num2 != 0)
                {
                    result = num1 / num2;
                }
                break;
        }
        
        _operations.Add(new Operation
        {
            Operand1 = num1,
            Operand2 = num2,
            OperationType = GetOperationType(op),
            Result = result
        });
        
        return result;
    }

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
    
    public void SaveOperationHistoryToFile()
    {
        using StreamWriter file = File.CreateText("calculatorlog.json");
        using JsonTextWriter writer = new JsonTextWriter(file)
        {
            Formatting = Formatting.Indented
        };

        JsonSerializer serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented,
        };

        writer.WriteStartObject();
        writer.WritePropertyName("Operations");
        serializer.Serialize(writer, _operations);
        writer.WriteEndObject();
    }
}