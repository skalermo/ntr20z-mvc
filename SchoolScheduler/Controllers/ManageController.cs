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
            Option selected;
            if (TempData["selected"] != null)
            {
                selected = (Option)TempData["selected"];
            }
            else
            {
                selected = Option.Rooms;
            }

            OptionList optionList = new OptionList();
            Data data = new Serde().deserialize("data.json");

            switch (selected)
            {
                case Option.Rooms:
                    optionList.values = data.Rooms;
                    break;
                case Option.Groups:
                    optionList.values = data.Groups;
                    break;
                case Option.Classes:
                    optionList.values = data.Classes;
                    break;
                case Option.Teachers:
                    optionList.values = data.Teachers;
                    break;
            }
            optionList.selected = selected;

            return View(optionList);
        }

        [HttpPost]
        public ActionResult SelectOption(Option selected)
        {
            TempData["selected"] = selected;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Option selected, string valueToDelete)
        {
            Serde serde = new Serde();
            Data data = serde.deserialize("data.json");
            data.Delete(selected, valueToDelete);
            serde.serialize(data, "data.json");

            TempData["selected"] = selected;
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Add(Option selected, string newValue)
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