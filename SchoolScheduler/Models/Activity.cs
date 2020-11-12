using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int ClassGroupId { get; set; }
        public int RoomId { get; set; }
        public int SlotId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ClassGroup ClassGroup { get; set; }
        public virtual Room Room { get; set; }
        public virtual Slot Slot { get; set; }
    }
}