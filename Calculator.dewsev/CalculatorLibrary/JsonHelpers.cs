namespace CalculatorLibrary;

using System;
using System.IO;
using Newtonsoft.Json;

internal static class JsonHelpers
{
    public static void SaveToJsonFile(object obj, string fileName)
    {
        File.WriteAllText(fileName, JsonConvert.SerializeObject(obj));
    }
    
    public static T ReadFromJsonFile<T>(string fileName) where T : new()
    {
        if (!File.Exists(fileName))
        {
            return new T();
        }

        try
        {
            string jsonString = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(jsonString) ?? new T();
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