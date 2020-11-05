using System.ComponentModel.Design.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchoolScheduler.Models
{
    public enum ManageOption
    {
        Rooms,
        Groups,
        Classes,
        Teachers
    }

    public class ManageOptionList
    {
        public ManageOptionList()
        {
            selectedOption = ManageOption.Rooms;
        }
        public ManageOption selectedOption { get; set; }
        public List<string> values { get; set; }
    }
    public enum ActivityOption
    {
        Rooms,
        Groups,
        Teachers
    }

    public class ActivityOptionList
    {
        public ActivityOptionList()
        {
            selectedOption = ActivityOption.Rooms;
        }
        public ActivityOption selectedOption { get; set; }
        public string selectedValue { get; set; }
        public List<string> values { get; set; }
    }
}
