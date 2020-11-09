using System.ComponentModel.Design.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public enum OptionEnum
    {
        Rooms,
        Groups,
        Classes,
        Teachers
    }

    public class OptionList
    {
        public OptionList()
        {
            selectedOption = OptionEnum.Rooms;
        }
        public OptionEnum selectedOption { get; set; }
        public string selectedValue { get; set; }
        public List<string> values { get; set; }
    }
}
