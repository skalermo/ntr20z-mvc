using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public class Serde
    {
        public Data deserialize(string jsonDatafile)
        {
            string jsonString = System.IO.File.ReadAllText(jsonDatafile);
            Data data = JsonSerializer.Deserialize<Data>(jsonString);

            return data;
        }

        public void serialize(Data data, string jsonDatafile)
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