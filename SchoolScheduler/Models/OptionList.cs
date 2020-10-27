using System.ComponentModel.Design.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public class OptionList
    {
        public OptionList()
        {
            options = new List<Option> { Option.Rooms, Option.Groups, Option.Classes, Option.Teachers };
            selected = Option.Rooms;
        }
        public List<Option> options;
        public Option selected { get; set; }
        public List<string> values { get; set; }
    }

    public enum Option
    {
        Rooms,
        Groups,
        Classes,
        Teachers
    }
}
