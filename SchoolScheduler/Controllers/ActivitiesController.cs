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
            const int rows = 9;
            const int cols = 5;

            Data data = new Serde().deserialize("data.json");

            List<Activity> activities = new List<Activity>();
            for (int i = 0; i < rows * cols; i++)
            {
                activities.Add(new Activity());
            }
            foreach (var item in data.Activities)
            {
                activities[item.Slot] = item;
                activities[item.Slot].isEmpty = false;
            }
            data.Activities = activities;

            return View(data);
        }

    }
}