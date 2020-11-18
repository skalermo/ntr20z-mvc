using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public enum OptionEnum
    {
        Rooms,
        ClassGroups,
        Subjects,
        Teachers
    }

    public class OptionList
    {
        public OptionList()
        {
            selectedOption = OptionEnum.Rooms;
        }
        public OptionEnum selectedOption { get; set; }
        public Entity selectedEntity { get; set; }
        public List<Entity> entities { get; set; }
    }
}
