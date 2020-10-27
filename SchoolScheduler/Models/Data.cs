using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public class Data
    {
        [JsonPropertyName("rooms")]
        public List<string> Rooms { get; set; }

        [JsonPropertyName("groups")]
        public List<string> Groups { get; set; }

        [JsonPropertyName("classes")]
        public List<string> Classes { get; set; }

        [JsonPropertyName("teachers")]
        public List<string> Teachers { get; set; }

        [JsonPropertyName("activities")]
        public List<Activity> Activities { get; set; }
        public bool Delete(Option selectedOption, string valueToDelete)
        {
            List<string> values = new List<string>();
            switch (selectedOption)
            {
                case Option.Rooms:
                    Rooms.Remove(valueToDelete);
                    break;
                case Option.Groups:
                    Groups.Remove(valueToDelete);
                    break;
                case Option.Classes:
                    Classes.Remove(valueToDelete);
                    break;
                case Option.Teachers:
                    Teachers.Remove(valueToDelete);
                    break;
            }

            return true;
        }
    }
}
