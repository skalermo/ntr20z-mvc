using System.Linq;
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

        public bool Add(Option selectedOption, string valueToAdd)
        {
            switch (selectedOption)
            {
                case Option.Rooms:
                    Rooms.Insert(0, valueToAdd);
                    break;
                case Option.Groups:
                    Groups.Insert(0, valueToAdd);
                    break;
                case Option.Classes:
                    Classes.Insert(0, valueToAdd);
                    break;
                case Option.Teachers:
                    Teachers.Insert(0, valueToAdd);
                    break;
            }

            return true;
        }
    }
}
