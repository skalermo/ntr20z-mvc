using System.Text.Json;

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
    }
}