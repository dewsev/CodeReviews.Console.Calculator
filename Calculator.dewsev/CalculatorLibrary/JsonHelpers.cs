using System;
using System.IO;
using Newtonsoft.Json;

namespace CalculatorLibrary;

internal static class JsonHelpers
{
    public static void SaveToJsonFile(object obj, string fileName)
    {
        string historyJson = JsonConvert.SerializeObject(obj);

        using StreamWriter file = File.CreateText(fileName);
        using JsonTextWriter writer = new JsonTextWriter(file);
        writer.Formatting = Formatting.Indented;
        
        writer.WriteRaw(historyJson);
    }
    
    public static T ReadFromJsonFile<T>(string fileName) where T: new()
    {
        if (!File.Exists(fileName))
        {
            return new T();
        }

        try
        {
            string historyJsonString = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(historyJsonString) ?? new T();
        }
        catch (Exception ex)
        {
            if (ex is JsonException or IOException)
            {
                return new T();
            }

            throw;
        }
    }
}