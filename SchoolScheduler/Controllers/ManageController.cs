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
            ManageOption selected = ManageOption.Rooms;
            if (TempData["selected"] != null)
            {
                selected = (ManageOption)TempData["selected"];
            }

            ManageOptionList optionList = new ManageOptionList();
            Data data = new Serde().deserialize("data.json");

            switch (selected)
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
            optionList.selected = selected;

            return View(optionList);
        }

        [HttpPost]
        public ActionResult SelectOption(ManageOption selected)
        {
            TempData["selected"] = selected;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(ManageOption selected, string valueToDelete)
        {
            Serde serde = new Serde();
            Data data = serde.deserialize("data.json");
            data.Delete(selected, valueToDelete);
            serde.serialize(data, "data.json");

            TempData["selected"] = selected;
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Add(ManageOption selected, string newValue)
        {

            Serde serde = new Serde();
            Data data = serde.deserialize("data.json");
            data.Add(selected, newValue);
            serde.serialize(data, "data.json");

            TempData["selected"] = selected;
            return RedirectToAction("Index");
        }
    }
}