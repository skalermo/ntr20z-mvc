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
            ManageOption selectedOption = ManageOption.Rooms;
            if (TempData["selected"] != null)
            {
                selectedOption = (ManageOption)TempData["selected"];
            }

            ManageOptionList optionList = new ManageOptionList();
            Data data = new Serde().deserialize("data.json");

            switch (selectedOption)
            {
                case ManageOption.Rooms:
                    optionList.values = data.Rooms;
                    break;
                case ManageOption.Groups:
                    optionList.values = data.Groups;
                    break;
                case ManageOption.Classes:
                    optionList.values = data.Classes;
                    break;
                case ManageOption.Teachers:
                    optionList.values = data.Teachers;
                    break;
            }
            optionList.selectedOption = selectedOption;

            return View(optionList);
        }

        [HttpPost]
        public ActionResult SelectOption(ManageOption selectedOption)
        {
            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(ManageOption selectedOption, string valueToDelete)
        {
            Serde serde = new Serde();
            Data data = serde.deserialize("data.json");
            data.Delete(selectedOption, valueToDelete);
            serde.serialize(data, "data.json");

            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Add(ManageOption selectedOption, string newValue)
        {

            Serde serde = new Serde();
            Data data = serde.deserialize("data.json");
            data.Add(selectedOption, newValue);
            serde.serialize(data, "data.json");

            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
        }
    }
}