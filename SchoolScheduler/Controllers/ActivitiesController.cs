using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchoolScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolScheduler.Controllers
{
    public class ActivitiesController : Controller
    {
        // GET: Activities
        public ActionResult Index()
        {
            OptionEnum selectedOption = OptionEnum.Rooms;
            if (TempData["selectedOption"] != null)
            {
                selectedOption = (OptionEnum)TempData["selectedOption"];
            }

            var optionList = new OptionList();
            optionList.entities = new List<Entity>();

            using (var context = new SchoolContext())
            {
                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                        optionList.entities = context.Rooms.Cast<Entity>().ToList();
                        break;
                    case OptionEnum.ClassGroups:
                        optionList.entities = context.ClassGroups.Cast<Entity>().ToList();
                        break;
                    case OptionEnum.Subjects:
                        optionList.entities = context.Subjects.Cast<Entity>().ToList();
                        break;
                    case OptionEnum.Teachers:
                        optionList.entities = context.Teachers.Cast<Entity>().ToList();
                        break;
                }
            }

            Entity selectedEntity = new Entity();
            if (Convert.ToInt32(TempData.Peek("selectedEntityId")) > 0)
            {
                int selectedEntityId = Convert.ToInt32(TempData["selectedEntityId"]);
                selectedEntity = optionList.entities.Where(ent => ent.Id == selectedEntityId).Single();
            }
            else if (optionList.entities.Any())
            {
                selectedEntity = optionList.entities[0];
            }

            optionList.selectedOption = selectedOption;
            optionList.selectedEntity = selectedEntity;

            var activityLabels = GenerateLabels(selectedOption, selectedEntity.Id);
            // ViewBag.selected = new ActivityFilterOptionList();

            ViewBag.activityLabels = activityLabels;

            ViewBag.optionList = optionList;
            return View();
        }

        [HttpPost]
        public ActionResult SelectOption(OptionEnum selectedOption)
        {
            TempData["selectedOption"] = selectedOption;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectOptionValue(OptionEnum selectedOption, int selectedEntity)
        {
            TempData["selectedOption"] = selectedOption;
            TempData["selectedEntityId"] = selectedEntity;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ActivityModal(OptionEnum selectedOption, string selectedEntity, int idx, int slot)
        {

            Activity activity;
            Slot chosenSlot;
            using (var context = new SchoolContext())
            {
                chosenSlot = context.Slots.Find(slot);
            }
            if (idx > 0)
            {
                using (var context = new SchoolContext())
                {
                    activity = context.Activities.Find(idx);

                }
            }
            else
            {
                activity = new Activity();
            }

            activity.Slot = chosenSlot;

            ViewBag.selectedOption = selectedOption;
            ViewBag.selectedEntity = selectedEntity;
            ViewBag.idx = idx;

            // ViewBag.classes = data.Classes;

            // ViewBag.rooms = data.Rooms;
            // ViewBag.groups = data.Groups;
            // ViewBag.teachers = data.Teachers;

            // var activitiesWithSameSlot = data.Activities.Where((v, i) => i != idx).ToList().FindAll(a => a.Slot == slot);
            // var roomsToExclude = activitiesWithSameSlot.Select(a => a.Room).ToHashSet();
            // var groupsToExclude = activitiesWithSameSlot.Select(a => a.Group).ToHashSet();
            // var teachersToExclude = activitiesWithSameSlot.Select(a => a.Teacher).ToHashSet();

            // if (selectedOption != OptionEnum.Rooms)
            // {
            //     ViewBag.rooms = data.Rooms.Except(roomsToExclude);
            // }
            // if (selectedOption != OptionEnum.Groups)
            // {
            //     ViewBag.groups = data.Groups.Except(groupsToExclude);
            // }
            // if (selectedOption != OptionEnum.Teachers)
            // {
            //     ViewBag.teachers = data.Teachers.Except(teachersToExclude);
            // }

            return PartialView(activity);
        }

        [HttpPost]
        public ActionResult ModalAction(OptionEnum selectedOption, string selectedValue,
        int idx, string room, string group, string class_, string teacher, int slot)
        {
            // if (Request.Form.ContainsKey("deleteButton"))
            // {
            //     DeleteActivity(idx);
            // }
            // else if (Request.Form.ContainsKey("saveButton"))
            // {
            //     var activity = new Activity()
            //     {
            //         Room = room,
            //         Group = group,
            //         Class = class_,
            //         Teacher = teacher,
            //         Slot = slot
            //     };

            //     if (idx == -1)
            //     {
            //         AddNewActivity(activity);
            //     }
            //     else
            //     {
            //         EditActivity(activity, idx);
            //     }
            // }

            TempData["selectedOption"] = selectedOption;
            TempData["selectedEntityId"] = selectedValue;
            return RedirectToAction("Index");
        }

        private void DeleteActivity(int idx)
        {
            // var data = JsonSerde.GetData();
            // data.Activities.RemoveAt(idx);
            // JsonSerde.SaveChanges(data);
        }

        private void AddNewActivity(Activity activity)
        {
            // var data = JsonSerde.GetData();
            // data.Activities.Add(activity);
            // JsonSerde.SaveChanges(data);
        }

        private void EditActivity(Activity activity, int idx)
        {
            // var data = JsonSerde.GetData();
            // data.Activities[idx] = activity;
            // JsonSerde.SaveChanges(data);
        }

        // public static List<Tuple<Activity, int>> getFilteredActivities(OptionEnum selectedOption, string selectedValue)
        // {
        //     // var data = JsonSerde.GetData();
        //     var filteredActivities = new List<Tuple<Activity, int>>();

        //     // int i = 0;
        //     // foreach (var activity in data.Activities)
        //     // {
        //     //     string value;
        //     //     switch (selectedOption)
        //     //     {
        //     //         case OptionEnum.Rooms:
        //     //         default:
        //     //             value = activity.Room;
        //     //             break;
        //     //         case OptionEnum.Groups:
        //     //             value = activity.Group;
        //     //             break;
        //     //         case OptionEnum.Classes:
        //     //             value = activity.Class;
        //     //             break;
        //     //         case OptionEnum.Teachers:
        //     //             value = activity.Teacher;
        //     //             break;
        //     //     }
        //     //     if (value == selectedValue)
        //     //     {
        //     //         filteredActivities.Add(Tuple.Create(activity, i));
        //     //     }
        //     //     i++;
        //     // }
        //     return filteredActivities;
        // }

        private List<Tuple<string, int>> GenerateLabels(OptionEnum selectedOption, int entityId)
        {
            List<Activity> activities = null;
            using (var context = new SchoolContext())
            {
                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                    default:
                        if (context.Rooms.Any())
                            activities = context.Rooms
                                .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                                .Where(ent => ent.Id == entityId)
                                .Select(ent => ent.Activities)
                                .Single();
                        break;
                    case OptionEnum.ClassGroups:
                        if (context.ClassGroups.Any())
                            activities = context.ClassGroups
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                             .Where(ent => ent.Id == entityId)
                             .Select(ent => ent.Activities)
                             .Single();
                        break;
                    case OptionEnum.Subjects:
                        if (context.Subjects.Any())
                            activities = context.Subjects
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                                 .Where(ent => ent.Id == entityId)
                                 .Select(ent => ent.Activities)
                                 .Single();
                        break;
                    case OptionEnum.Teachers:
                        if (context.Teachers.Any())
                            activities = context.Teachers
                                .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(ent => ent.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                                .Where(ent => ent.Id == entityId)
                                .Select(ent => ent.Activities)
                                .Single();
                        break;
                }
            }
            if (activities == null)
                activities = new List<Activity>();

            const int rows = 9;
            const int cols = 5;
            var labels = new List<Tuple<string, int>>();
            for (int i = 0; i < rows * cols; i++)
            {
                labels.Add(Tuple.Create("", 0));
            }

            foreach (var activity in activities)
            {
                string strToShow;
                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                    default:
                        strToShow = activity.ClassGroup.Name;
                        break;
                    case OptionEnum.ClassGroups:
                        strToShow = activity.Room.Name + " " + activity.Subject.Name;
                        break;
                    case OptionEnum.Teachers:
                        strToShow = activity.Room.Name + " " + activity.Subject.Name + " " + activity.ClassGroup.Name;
                        break;
                }
                labels[activity.SlotId] = Tuple.Create(strToShow, activity.SlotId);
            }
            return labels;
        }
    }
}