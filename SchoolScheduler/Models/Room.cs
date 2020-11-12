using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public virtual List<Activity> Activitites { get; set; }
    }
}