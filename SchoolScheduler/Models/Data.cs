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
        public bool Delete(ManageOption selectedOption, string valueToDelete)
        {
            switch (selectedOption)
            {
                case ManageOption.Rooms:
                    Rooms.Remove(valueToDelete);
                    break;
                case ManageOption.Groups:
                    Groups.Remove(valueToDelete);
                    break;
                case ManageOption.Classes:
                    Classes.Remove(valueToDelete);
                    break;
                case ManageOption.Teachers:
                    Teachers.Remove(valueToDelete);
                    break;
            }

            return true;
        }

        public bool Add(ManageOption selectedOption, string valueToAdd)
        {
            switch (selectedOption)
            {
                case ManageOption.Rooms:
                    Rooms.Insert(0, valueToAdd);
                    break;
                case ManageOption.Groups:
                    Groups.Insert(0, valueToAdd);
                    break;
                case ManageOption.Classes:
                    Classes.Insert(0, valueToAdd);
                    break;
                case ManageOption.Teachers:
                    Teachers.Insert(0, valueToAdd);
                    break;
            }

            return true;
        }
    }
}
