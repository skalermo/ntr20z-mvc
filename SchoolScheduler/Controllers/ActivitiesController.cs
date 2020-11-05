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
            ActivityOption selectedOption = ActivityOption.Rooms;
            if (TempData["selectedOption"] != null)
            {
                selectedOption = (ActivityOption)TempData["selectedOption"];
            }

            var optionList = new ActivityOptionList();
            Data data = new Serde().deserialize("data.json");

            switch (selectedOption)
            {
                case ActivityOption.Rooms:
                    optionList.values = data.Rooms;
                    break;
                case ActivityOption.Groups:
                    optionList.values = data.Groups;
                    break;
                case ActivityOption.Teachers:
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
        public ActionResult SelectOption(ActivityOption selectedOption)
        {
            TempData["selectedOption"] = selectedOption;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectOptionValue(ActivityOption selectedOption, string selectedValue)
        {
            TempData["selectedOption"] = selectedOption;
            TempData["selectedValue"] = selectedValue;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ActivityModal(ActivityOption selectedOption, string selectedValue, int slot)
        {
            Data data = new Serde().deserialize("data.json");
            var filteredActivities = getFilteredActivities(selectedOption, selectedValue);
            var filtered = filteredActivities.Where(a => a.Slot == slot);
            Activity activity;
            if (filtered.Any())
            {
                activity = filtered.ElementAt(0);
            }
            else
            {
                activity = new Activity();
            }

            ViewBag.selectedOption = selectedOption;
            ViewBag.selectedValue = selectedValue;
            ViewBag.rooms = data.Rooms;
            ViewBag.classes = data.Classes;
            ViewBag.groups = data.Groups;
            ViewBag.teachers = data.Teachers;

            return PartialView(activity);
        }

        [HttpGet]
        private List<Activity> getFilteredActivities(ActivityOption selectedOption, string selectedValue)
        {

            Data data = new Serde().deserialize("data.json");
            var filteredActivities = new List<Activity>();

            foreach (var activity in data.Activities)
            {
                string value;
                switch (selectedOption)
                {
                    case ActivityOption.Rooms:
                    default:
                        value = activity.Room;
                        break;
                    case ActivityOption.Groups:
                        value = activity.Group;
                        break;
                    case ActivityOption.Teachers:
                        value = activity.Teacher;
                        break;
                }
                if (value == selectedValue)
                {
                    filteredActivities.Add(activity);
                }
            }
            return filteredActivities;
        }

        private List<string> GenerateLabels(ActivityOption selectedOption, string selectedValue)
        {
            var filteredActivities = getFilteredActivities(selectedOption, selectedValue);

            const int rows = 9;
            const int cols = 5;
            var labels = new List<string>(new string[rows * cols]);
            for (int i = 0; i < rows * cols; i++)
            {
                labels[i] = "";
            }

            foreach (var activity in filteredActivities)
            {
                string strToShow;
                switch (selectedOption)
                {
                    case ActivityOption.Rooms:
                    default:
                        strToShow = activity.Group;
                        break;
                    case ActivityOption.Groups:
                        strToShow = activity.Room + " " + activity.Class;
                        break;
                    case ActivityOption.Teachers:
                        strToShow = activity.Room + " " + activity.Class + " " + activity.Group;
                        break;
                }
                labels[activity.Slot] = strToShow;
            }
            return labels;
        }
    }
}