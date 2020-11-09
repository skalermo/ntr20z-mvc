using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public class JsonSerde
    {
        public static Data GetData(string jsonDatafile = "data.json")
        {
            string jsonString = System.IO.File.ReadAllText(jsonDatafile);
            Data data = JsonSerializer.Deserialize<Data>(jsonString);

            return data;
        }

        public static void SaveChanges(Data data, string jsonDatafile = "data.json")
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(data, options);
            System.IO.File.WriteAllText(jsonDatafile, jsonString);
        }
    }
}