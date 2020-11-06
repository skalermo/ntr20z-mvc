using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolScheduler.Models;

namespace SchoolScheduler.Controllers
{
    public class ActivitiesController : Controller
    {
        // GET: Activities
        public ActionResult Index()
        {
            OptionEnum selectedOption = OptionEnum.Rooms;
            if (TempData["selectedOption"] != null)
            {
                selectedOption = (OptionEnum)TempData["selectedOption"];
            }

            var optionList = new OptionList();
            Data data = new Serde().deserialize("data.json");

            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                    optionList.values = data.Rooms;
                    break;
                case OptionEnum.Groups:
                    optionList.values = data.Groups;
                    break;
                case OptionEnum.Teachers:
                    optionList.values = data.Teachers;
                    break;
            }
            string selectedValue = "";
            if (optionList.values.Any())
            {
                selectedValue = optionList.values[0];
            }
            if (TempData["selectedValue"] != null)
            {
                selectedValue = (string)TempData["selectedValue"];
            }
            optionList.selectedOption = selectedOption;
            optionList.selectedValue = selectedValue;


            var activityLabels = GenerateLabels(selectedOption, selectedValue);
            // ViewBag.selected = new ActivityFilterOptionList();

            ViewBag.activityLabels = activityLabels;

            return View(optionList);
        }

        [HttpPost]
        public ActionResult SelectOption(OptionEnum selectedOption)
        {
            TempData["selectedOption"] = selectedOption;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectOptionValue(OptionEnum selectedOption, string selectedValue)
        {
            TempData["selectedOption"] = selectedOption;
            TempData["selectedValue"] = selectedValue;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ActivityModal(OptionEnum selectedOption, string selectedValue, int idx, int slot)
        {
            Data data = new Serde().deserialize("data.json");

            Activity activity;
            if (idx >= 0)
            {
                activity = data.Activities[idx];
            }
            else
            {
                activity = new Activity();
            }
            activity.Slot = slot;

            ViewBag.selectedOption = selectedOption;
            ViewBag.selectedValue = selectedValue;
            ViewBag.idx = idx;
            ViewBag.rooms = data.Rooms;
            ViewBag.classes = data.Classes;
            ViewBag.groups = data.Groups;
            ViewBag.teachers = data.Teachers;

            return PartialView(activity);
        }

        [HttpPost]
        public ActionResult ModalAction(OptionEnum selectedOption, string selectedValue,
        int idx, string room, string group, string class_, string teacher, int slot)
        {
            if (Request.Form.ContainsKey("deleteButton"))
            {
                DeleteActivity(idx);
            }
            else if (Request.Form.ContainsKey("saveButton"))
            {
                var activity = new Activity()
                {
                    Room = room,
                    Group = group,
                    Class = class_,
                    Teacher = teacher,
                    Slot = slot
                };

                if (idx == -1)
                {
                    AddNewActivity(activity);
                }
                else
                {
                    EditActivity(activity, idx);
                }
            }

            TempData["selectedOption"] = selectedOption;
            TempData["selectedValue"] = selectedValue;
            return RedirectToAction("Index");
        }

        private void DeleteActivity(int idx)
        {
            var data = new Serde().deserialize("data.json");
            data.Activities.RemoveAt(idx);
            new Serde().serialize(data, "data.json");
        }

        private void AddNewActivity(Activity activity)
        {
            var data = new Serde().deserialize("data.json");
            data.Activities.Add(activity);
            new Serde().serialize(data, "data.json");
        }

        private void EditActivity(Activity activity, int idx)
        {
            var data = new Serde().deserialize("data.json");
            data.Activities[idx] = activity;
            new Serde().serialize(data, "data.json");
        }

        public static List<Tuple<Activity, int>> getFilteredActivities(OptionEnum selectedOption, string selectedValue)
        {

            Data data = new Serde().deserialize("data.json");
            var filteredActivities = new List<Tuple<Activity, int>>();

            int i = 0;
            foreach (var activity in data.Activities)
            {
                string value;
                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                    default:
                        value = activity.Room;
                        break;
                    case OptionEnum.Groups:
                        value = activity.Group;
                        break;
                    case OptionEnum.Teachers:
                        value = activity.Teacher;
                        break;
                }
                if (value == selectedValue)
                {
                    filteredActivities.Add(Tuple.Create(activity, i));
                }
                i++;
            }
            return filteredActivities;
        }

        private List<Tuple<string, int>> GenerateLabels(OptionEnum selectedOption, string selectedValue)
        {
            var filteredActivities = getFilteredActivities(selectedOption, selectedValue);

            const int rows = 9;
            const int cols = 5;
            var labels = new List<Tuple<string, int>>();
            for (int i = 0; i < rows * cols; i++)
            {
                labels.Add(Tuple.Create("", -1));
            }

            foreach (var activity in filteredActivities)
            {
                string strToShow;
                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                    default:
                        strToShow = activity.Item1.Group;
                        break;
                    case OptionEnum.Groups:
                        strToShow = activity.Item1.Room + " " + activity.Item1.Class;
                        break;
                    case OptionEnum.Teachers:
                        strToShow = activity.Item1.Room + " " + activity.Item1.Class + " " + activity.Item1.Group;
                        break;
                }
                labels[activity.Item1.Slot] = Tuple.Create(strToShow, activity.Item2);
            }
            return labels;
        }
    }
}