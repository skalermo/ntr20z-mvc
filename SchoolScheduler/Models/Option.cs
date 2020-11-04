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
            selected = ManageOption.Rooms;
        }
        public ManageOption selected { get; set; }
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
            selected = ActivityOption.Rooms;
        }
        public ActivityOption selected { get; set; }
        public List<string> values { get; set; }
    }
}
