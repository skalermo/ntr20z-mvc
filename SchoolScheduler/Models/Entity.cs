using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Activity> Activities { get; set; }
    }
}