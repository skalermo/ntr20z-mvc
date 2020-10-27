using System.Net;
using System;
using SchoolScheduler.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SchoolScheduler.Controllers
{
    public class ManageController : Controller
    {
        public ActionResult Index(Option selected)
        {
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

        public ActionResult Delete(Option selected, string valueToDelete)
        {
            Serde serde = new Serde();
            Data data = serde.deserialize("data.json");
            data.Delete(selected, valueToDelete);
            serde.serialize(data, "data.json");

            return RedirectToAction("Index", new { selected });

        }
    }
}