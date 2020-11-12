using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class ClassGroup
    {
        public int ClassGroupId { get; set; }
        public string Name { get; set; }
        public virtual List<Activity> Activitites { get; set; }
    }
}