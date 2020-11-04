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
            selected = Option.Rooms;
        }
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

    public class ActivityFilterOptionList
    {
        public ActivityFilterOptionList()
        {
            selected = ActivityFilterOption.Rooms;
        }
        public ActivityFilterOption selected { get; set; }
        public List<string> values { get; set; }
    }

    public enum ActivityFilterOption
    {
        Rooms,
        Groups,
        Teachers
    }

}
