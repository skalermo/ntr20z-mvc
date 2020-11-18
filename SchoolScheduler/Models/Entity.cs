using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Activity> Activities { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}