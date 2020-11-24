using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchoolScheduler.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SchoolScheduler.Controllers
{
    public class ActivitiesController : Controller
    {
        // GET: Activities
        public ActionResult Index()
        {
            if (!SchoolContext.CanConnect())
                return RedirectToAction("Index", "Error");

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

            // Entity selectedEntity = null;
            // if (Convert.ToInt32(TempData.Peek("selectedEntityId")) > 0)
            // {
            int selectedEntityId = Convert.ToInt32(TempData["selectedEntityId"]);
            Entity selectedEntity = optionList.entities.Where(ent => ent.Id == selectedEntityId).SingleOrDefault();
            // }
            if (selectedEntity == null)
            {
                if (selectedEntityId > 0)
                    TempData["Alert"] = "Concurrency warning: selected entity was already deleted";
                if (optionList.entities.Any())
                    selectedEntity = optionList.entities[0];
                else
                    selectedEntity = new Entity();
            }

            optionList.selectedOption = selectedOption;
            optionList.selectedEntity = selectedEntity;

            var activityLabels = GenerateLabels(selectedOption, selectedEntity.Id);

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
        public ActionResult SelectOptionValue(OptionEnum selectedOption, int selectedEntityId)
        {
            TempData["selectedOption"] = selectedOption;
            TempData["selectedEntityId"] = selectedEntityId;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> ActivityModal(OptionEnum selectedOption, int selectedEntityId, int idx, int slot)
        {

            Activity activity;
            Slot chosenSlot;
            Entity entity;
            using (var context = new SchoolContext())
            {
                chosenSlot = await context.Slots.FindAsync(slot);
                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                    default:
                        entity = await context.Rooms.FindAsync(selectedEntityId);
                        break;
                    case OptionEnum.ClassGroups:
                        entity = await context.ClassGroups.FindAsync(selectedEntityId);
                        break;
                    case OptionEnum.Teachers:
                        entity = await context.Teachers.FindAsync(selectedEntityId);
                        break;
                }
            }

            // Selected entity from the dropdown 
            // which was then deleted by another user
            if (entity == null)
                entity = new Entity() { Name = "", Id = selectedEntityId };
            // entity.Name = "";
            // entity.Id = selectedEntityId;
            //     return PartialView(new Activity());

            if (idx > 0)
            {
                using (var context = new SchoolContext())
                {
                    activity = await context.Activities
                    .Include(activity => activity.Room)
                    .Include(activity => activity.ClassGroup)
                    .Include(activity => activity.Subject)
                    .Include(activity => activity.Teacher)
                    .Where(activity => activity.ActivityId == idx)
                    .SingleAsync();
                }
            }
            else
            {
                activity = new Activity();
            }

            activity.SlotId = chosenSlot.SlotId;

            ViewBag.selectedOption = selectedOption;
            ViewBag.selectedEntity = entity;
            ViewBag.idx = idx;

            using (var db = new SchoolContext())
            {
                if (selectedOption != OptionEnum.Rooms)
                    ViewBag.Rooms = await db.Rooms
                    .Include(r => r.Activities)
                    .Where(r => r.Activities.All(a => a.ActivityId == idx || a.SlotId != slot))
                    .ToListAsync();
                else
                    ViewBag.Rooms = await db.Rooms.ToListAsync();

                if (selectedOption != OptionEnum.ClassGroups)
                    ViewBag.ClassGroups = await db.ClassGroups
                        .Include(cg => cg.Activities)
                        .Where(cg => cg.Activities.All(a => a.ActivityId == idx || a.SlotId != slot))
                        .ToListAsync();
                else
                    ViewBag.ClassGroups = await db.ClassGroups.ToListAsync();

                ViewBag.Subjects = await db.Subjects.ToListAsync();

                if (selectedOption != OptionEnum.Teachers)
                    ViewBag.Teachers = await db.Teachers
                    .Include(t => t.Activities)
                    .Where(t => t.Activities.All(a => a.ActivityId == idx || a.SlotId != slot))
                    .ToListAsync();
                else
                    ViewBag.Teachers = await db.Teachers.ToListAsync();
            }

            return PartialView(activity);
        }

        [HttpPost]
        public ActionResult ModalAction(OptionEnum selectedOption, int selectedEntityId,
        int activityId, int roomId, int classGroupId, int subjectId, int teacherId, int slotId,
        byte timestamp)
        {
            if (Request.Form.ContainsKey("deleteButton"))
            {
                DeleteActivity(activityId);
            }
            else if (Request.Form.ContainsKey("saveButton"))
            {
                using (var db = new SchoolContext())
                {
                    Activity activity = db.Activities.Find(activityId);
                    if (activity == null)
                    {
                        activity = new Activity();
                        db.Activities.Add(activity);
                    }

                    // todo check if these fields are not already deleted
                    activity.RoomId = roomId;
                    activity.ClassGroupId = classGroupId;
                    activity.SubjectId = subjectId;
                    activity.TeacherId = teacherId;

                    activity.SlotId = slotId;

                    db.SaveChanges();
                }
            }

            TempData["selectedOption"] = selectedOption;
            TempData["selectedEntityId"] = selectedEntityId;
            return RedirectToAction("Index");
        }

        private void DeleteActivity(int activityId)
        {
            Activity activity = new Activity() { ActivityId = activityId };
            using (var db = new SchoolContext())
            {
                db.Activities.Attach(activity);
                db.Activities.Remove(activity);
                db.SaveChanges();
            }
        }

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
                                .Include(r => r.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(r => r.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(r => r.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                                .Where(r => r.Id == entityId)
                                .Select(r => r.Activities)
                                .Single();
                        break;
                    case OptionEnum.ClassGroups:
                        if (context.ClassGroups.Any())
                            activities = context.ClassGroups
                                 .Include(cg => cg.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(cg => cg.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(cg => cg.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                             .Where(cg => cg.Id == entityId)
                             .Select(cg => cg.Activities)
                             .Single();
                        break;
                    case OptionEnum.Subjects:
                        if (context.Subjects.Any())
                            activities = context.Subjects
                                 .Include(s => s.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(s => s.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(s => s.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                                 .Where(s => s.Id == entityId)
                                 .Select(s => s.Activities)
                                 .Single();
                        break;
                    case OptionEnum.Teachers:
                        if (context.Teachers.Any())
                            activities = context.Teachers
                                .Include(t => t.Activities)
                                    .ThenInclude(a => a.Room)
                                 .Include(t => t.Activities)
                                    .ThenInclude(a => a.Subject)
                                 .Include(t => t.Activities)
                                    .ThenInclude(a => a.ClassGroup)
                                .Where(t => t.Id == entityId)
                                .Select(t => t.Activities)
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
                labels[activity.SlotId - 1] = Tuple.Create(strToShow, activity.ActivityId);
            }
            return labels;
        }
    }
}