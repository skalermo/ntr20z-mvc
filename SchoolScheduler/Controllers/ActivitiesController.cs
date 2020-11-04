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
            ActivityOption selected = ActivityOption.Rooms;
            if (TempData["selected"] != null)
            {
                selected = (ActivityOption)TempData["selected"];
            }

            var optionList = new ActivityOptionList();
            Data data = new Serde().deserialize("data.json");

            switch (selected)
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
            optionList.selected = selected;

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

            // var tupleModel = new Tuple<ActivityFilterOptionList, List<Activity>>(optionList, activities);
            ViewBag.activities = activities;
            return View(optionList);
        }

        [HttpPost]
        public ActionResult ChooseOption(ActivityOption selected)
        {
            TempData["selected"] = selected;
            return RedirectToAction("Index");
        }
    }
}