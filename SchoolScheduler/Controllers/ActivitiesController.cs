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
        public PartialViewResult GetModalData()
        {
            return PartialView();
        }

        private List<string> GenerateLabels(ActivityOption selectedOption, string selectedValue)
        {
            Data data = new Serde().deserialize("data.json");

            const int rows = 9;
            const int cols = 5;
            var labels = new List<string>(new string[rows * cols]);
            for (int i = 0; i < rows * cols; i++)
            {
                labels[i] = "";
            }

            foreach (var activity in data.Activities)
            {
                string value;
                string strToShow;
                switch (selectedOption)
                {
                    case ActivityOption.Rooms:
                    default:
                        value = activity.Room;
                        strToShow = activity.Group;
                        break;
                    case ActivityOption.Groups:
                        value = activity.Group;
                        strToShow = activity.Room + " " + activity.Class;
                        break;
                    case ActivityOption.Teachers:
                        value = activity.Teacher;
                        strToShow = activity.Room + " " + activity.Class + " " + activity.Group;
                        break;
                }
                if (value == selectedValue)
                {
                    labels[activity.Slot] = strToShow;
                }
            }
            return labels;
        }
    }
}