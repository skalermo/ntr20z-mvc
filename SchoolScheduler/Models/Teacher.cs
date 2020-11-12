using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public virtual List<Activity> Activities { get; set; }
    }
}