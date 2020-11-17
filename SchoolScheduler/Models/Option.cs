using System.ComponentModel.Design.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
