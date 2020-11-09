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
            Data data = JsonSerde.GetData();

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
            Data data = JsonSerde.GetData();

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

            ViewBag.classes = data.Classes;

            ViewBag.rooms = data.Rooms;
            ViewBag.groups = data.Groups;
            ViewBag.teachers = data.Teachers;

            var activitiesWithSameSlot = data.Activities.Where((v, i) => i != idx).ToList().FindAll(a => a.Slot == slot);
            var roomsToExclude = activitiesWithSameSlot.Select(a => a.Room).ToHashSet();
            var groupsToExclude = activitiesWithSameSlot.Select(a => a.Group).ToHashSet();
            var teachersToExclude = activitiesWithSameSlot.Select(a => a.Teacher).ToHashSet();

            if (selectedOption != OptionEnum.Rooms)
            {
                ViewBag.rooms = data.Rooms.Except(roomsToExclude);
            }
            if (selectedOption != OptionEnum.Groups)
            {
                ViewBag.groups = data.Groups.Except(groupsToExclude);
            }
            if (selectedOption != OptionEnum.Teachers)
            {
                ViewBag.teachers = data.Teachers.Except(teachersToExclude);
            }

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
            var data = JsonSerde.GetData();
            data.Activities.RemoveAt(idx);
            JsonSerde.SaveChanges(data);
        }

        private void AddNewActivity(Activity activity)
        {
            var data = JsonSerde.GetData();
            data.Activities.Add(activity);
            JsonSerde.SaveChanges(data);
        }

        private void EditActivity(Activity activity, int idx)
        {
            var data = JsonSerde.GetData();
            data.Activities[idx] = activity;
            JsonSerde.SaveChanges(data);
        }

        public static List<Tuple<Activity, int>> getFilteredActivities(OptionEnum selectedOption, string selectedValue)
        {
            var data = JsonSerde.GetData();
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
                    case OptionEnum.Classes:
                        value = activity.Class;
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