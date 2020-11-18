using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class Slot
    {
        public int SlotId { get; set; }
        public string Name { get; set; }
        public virtual List<Activity> Activities { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}