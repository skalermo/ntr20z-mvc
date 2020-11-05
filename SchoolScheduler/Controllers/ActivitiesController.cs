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
            string selectedValue = "";
            if (TempData["selectedValue"] != null)
            {
                selectedValue = (string)TempData["selectedValue"];
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
            optionList.selectedOption = selectedOption;
            optionList.selectedValue = selectedValue;

            const int rows = 9;
            const int cols = 5;
            List<Activity> activities = new List<Activity>();
            for (int i = 0; i < rows * cols; i++)
            {
                activities.Add(new Activity());
            }
            foreach (var item in data.Activities)
            {
                activities[item.Slot] = item;
            }

            // ViewBag.selected = new ActivityFilterOptionList();

            ViewBag.activities = activities;

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
    }
}