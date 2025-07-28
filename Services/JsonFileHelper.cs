using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ExpensesSathi.Services
{
    public static class JsonFileHelper
    {
        public static void SaveToFile<T>(string filePath, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static List<T> LoadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();
            var json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }
}
