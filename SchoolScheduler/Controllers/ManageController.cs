using System.Net;
using System;
using SchoolScheduler.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SchoolScheduler.Controllers
{
    public class ManageController : Controller
    {
        public ActionResult Index()
        {
            OptionEnum selectedOption = OptionEnum.Rooms;
            if (TempData["selected"] != null)
            {
                selectedOption = (OptionEnum)TempData["selected"];
            }

            OptionList optionList = new OptionList();
            Data data = JsonSerde.GetData();

            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                    optionList.values = data.Rooms;
                    break;
                case OptionEnum.Groups:
                    optionList.values = data.Groups;
                    break;
                case OptionEnum.Classes:
                    optionList.values = data.Classes;
                    break;
                case OptionEnum.Teachers:
                    optionList.values = data.Teachers;
                    break;
            }
            optionList.selectedOption = selectedOption;

            return View(optionList);
        }

        [HttpPost]
        public ActionResult SelectOption(OptionEnum selectedOption)
        {
            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(OptionEnum selectedOption)
        {
            string valueToDelete = Request.Form["valueToDelete"];
            Data data = JsonSerde.GetData();

            var filteredActivities = ActivitiesController.getFilteredActivities(selectedOption, valueToDelete);
            if (filteredActivities.Count > 0)
            {
                TempData["Alert"] = "Used in an Activity.";
                TempData["selected"] = selectedOption;
                return RedirectToAction("Index");
            }

            data.Delete(selectedOption, valueToDelete);
            JsonSerde.SaveChanges(data);

            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Add(OptionEnum selectedOption, string newValue)
        {
            Data data = JsonSerde.GetData();
            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                    if (data.Rooms.Contains(newValue))
                    {
                        TempData["Alert"] = "Value already exists";
                        TempData["selected"] = selectedOption;
                        return RedirectToAction("Index");
                    }
                    break;
                case OptionEnum.Groups:
                    if (data.Groups.Contains(newValue))
                    {
                        TempData["Alert"] = "Value already exists";
                        TempData["selected"] = selectedOption;
                        return RedirectToAction("Index");
                    }
                    break;
                case OptionEnum.Classes:
                    if (data.Classes.Contains(newValue))
                    {
                        TempData["Alert"] = "Value already exists";
                        TempData["selected"] = selectedOption;
                        return RedirectToAction("Index");
                    }
                    break;
                case OptionEnum.Teachers:
                    if (data.Teachers.Contains(newValue))
                    {
                        TempData["Alert"] = "Value already exists";
                        TempData["selected"] = selectedOption;
                        return RedirectToAction("Index");
                    }
                    break;
            }
            data.Add(selectedOption, newValue);
            JsonSerde.SaveChanges(data);

            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
        }
    }
}