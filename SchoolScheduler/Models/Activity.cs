using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public class Activity
    {
        public Activity()
        {
            isEmpty = true;
        }
        [JsonPropertyName("room")]
        public string Room { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [JsonPropertyName("teacher")]
        public string Teacher { get; set; }

        public bool isEmpty { get; set; }
    }
}