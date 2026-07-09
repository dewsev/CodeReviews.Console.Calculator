using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CalculatorLibrary;

using System.IO;
using Newtonsoft.Json;

public class Calculator
{
    private const string TotalOperationsPerformedJsonTokenName = "TotalOperationsPerformed";
    private const string OperationsJsonTokenName = "Operations";
    private const string LogFileName = "calculatorlog.json";

    private List<Operation> _operations = [];
    
    private int _totalOperationsPerformed;
    
    public Calculator()
    {
        JObject json = LoadJObjectSafe(LogFileName);
        LoadTotalOperationsPerformedFromJson(json);
        LoadOperationHistoryFromJson(json);
    }
    
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

        _totalOperationsPerformed++;
        
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

    private void LoadOperationHistoryFromJson(JObject json)
    {
        JArray operations = json.GetValue(OperationsJsonTokenName) as JArray ?? [];
        _operations = operations.ToObject<List<Operation>>() ?? [];
    }
    
    private void LoadTotalOperationsPerformedFromJson(JObject json)
    {
        string count = json.GetValue(TotalOperationsPerformedJsonTokenName)?.ToString() ?? "0";
        _totalOperationsPerformed = int.TryParse(count, out int n) ? n : 0;
    }
    
    private JObject LoadJObjectSafe(string path)
    {
        if (!File.Exists(path)) return new JObject();
        
        string text = File.ReadAllText(LogFileName);
        if (string.IsNullOrWhiteSpace(text))
        {
            return new JObject();
        }

        try
        {
            using StreamReader file = File.OpenText(LogFileName);
            using JsonTextReader reader = new JsonTextReader(file);

            JToken token = JToken.ReadFrom(reader);
            return token as JObject ?? new JObject();
        }
        catch (Exception ex)
        {
            if (ex is JsonReaderException or IOException)
            {
                return new JObject();
            }

            throw;
        }
    }
    
    public void SaveOperationHistoryToFile()
    {
        using StreamWriter _logFile = File.CreateText(LogFileName);
        using JsonTextWriter writer = new JsonTextWriter(_logFile)
        {
            Formatting = Formatting.Indented
        };

        JsonSerializer serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented,
        };

        writer.WriteStartObject();
        writer.WritePropertyName(TotalOperationsPerformedJsonTokenName);
        writer.WriteValue(_totalOperationsPerformed);
        writer.WritePropertyName(OperationsJsonTokenName);
        serializer.Serialize(writer, _operations);
        writer.WriteEndObject();
    }
}
