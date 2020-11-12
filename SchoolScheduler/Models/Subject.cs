using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public virtual List<Activity> Activities { get; set; }
    }
}