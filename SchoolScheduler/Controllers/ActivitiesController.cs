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
        private SchoolContext db = new SchoolContext();

        // GET: Activities
        public async Task<ActionResult> Index()
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

            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                    optionList.entities = await db.Rooms.Cast<Entity>().ToListAsync();
                    break;
                case OptionEnum.ClassGroups:
                    optionList.entities = await db.ClassGroups.Cast<Entity>().ToListAsync();
                    break;
                case OptionEnum.Subjects:
                    optionList.entities = await db.Subjects.Cast<Entity>().ToListAsync();
                    break;
                case OptionEnum.Teachers:
                    optionList.entities = await db.Teachers.Cast<Entity>().ToListAsync();
                    break;
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
                    TempData["ConcurrencyAlert"] = "Selected entity was already deleted";
                if (optionList.entities.Any())
                    selectedEntity = optionList.entities[0];
                else
                    selectedEntity = new Entity();
            }

            optionList.selectedOption = selectedOption;
            optionList.selectedEntity = selectedEntity;

            var activityLabels = await GenerateLabels(selectedOption, selectedEntity.Id);

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

            Slot chosenSlot;
            Entity entity;

            chosenSlot = await db.Slots.FindAsync(slot);
            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                default:
                    entity = await db.Rooms.FindAsync(selectedEntityId);
                    break;
                case OptionEnum.ClassGroups:
                    entity = await db.ClassGroups.FindAsync(selectedEntityId);
                    break;
                case OptionEnum.Teachers:
                    entity = await db.Teachers.FindAsync(selectedEntityId);
                    break;
            }

            // Selected entity from the dropdown 
            // which was then deleted by another user
            if (entity == null)
                entity = new Entity() { Name = "", Id = selectedEntityId };
            // entity.Name = "";
            // entity.Id = selectedEntityId;
            //     return PartialView(new Activity());

            Activity activity = null;
            if (idx > 0)
            {
                activity = await db.Activities
                .Include(activity => activity.Room)
                .Include(activity => activity.ClassGroup)
                .Include(activity => activity.Subject)
                .Include(activity => activity.Teacher)
                .Where(activity => activity.ActivityId == idx)
                .SingleOrDefaultAsync();
            }

            if (activity == null)
            {
                activity = new Activity();
            }

            activity.SlotId = chosenSlot.SlotId;

            ViewBag.selectedOption = selectedOption;
            ViewBag.selectedEntity = entity;
            ViewBag.idx = idx;

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

            return PartialView(activity);
        }

        [HttpPost]
        public async Task<ActionResult> ModalAction(OptionEnum selectedOption, int selectedEntityId,
        int activityId, int roomId, int classGroupId, int subjectId, int teacherId, int slotId,
        byte timestamp)
        {
            if (Request.Form.ContainsKey("deleteButton"))
            {
                await DeleteActivity(activityId);
            }
            else if (Request.Form.ContainsKey("saveButton"))
            {
                Activity activity = await db.Activities.FindAsync(activityId);
                if (activity == null)
                {
                    activity = new Activity();
                    await db.Activities.AddAsync(activity);
                }

                // todo check if these fields are not already deleted
                activity.RoomId = roomId;
                activity.ClassGroupId = classGroupId;
                activity.SubjectId = subjectId;
                activity.TeacherId = teacherId;

                activity.SlotId = slotId;

                await db.SaveChangesAsync();
            }

            TempData["selectedOption"] = selectedOption;
            TempData["selectedEntityId"] = selectedEntityId;
            return RedirectToAction("Index");
        }

        private async Task DeleteActivity(int activityId)
        {
            Activity activityToDelete = await db.Activities.FindAsync(activityId);
            if (activityToDelete != null)
                try
                {
                    db.Entry(activityToDelete).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["ConcurrencyAlert"] = "Activity was already deleted";
                }
            else
                TempData["ConcurrencyAlert"] = "Activity was already deleted";
        }

        private async Task<List<Tuple<string, int>>> GenerateLabels(OptionEnum selectedOption, int entityId)
        {
            List<Activity> activities = null;
            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                default:
                    if (await db.Rooms.AnyAsync())
                        activities = await db.Rooms
                            .Include(r => r.Activities)
                                .ThenInclude(a => a.Room)
                             .Include(r => r.Activities)
                                .ThenInclude(a => a.Subject)
                             .Include(r => r.Activities)
                                .ThenInclude(a => a.ClassGroup)
                            .Where(r => r.Id == entityId)
                            .Select(r => r.Activities)
                            .SingleAsync();
                    break;
                case OptionEnum.ClassGroups:
                    if (await db.ClassGroups.AnyAsync())
                        activities = await db.ClassGroups
                             .Include(cg => cg.Activities)
                                .ThenInclude(a => a.Room)
                             .Include(cg => cg.Activities)
                                .ThenInclude(a => a.Subject)
                             .Include(cg => cg.Activities)
                                .ThenInclude(a => a.ClassGroup)
                         .Where(cg => cg.Id == entityId)
                         .Select(cg => cg.Activities)
                         .SingleAsync();
                    break;
                case OptionEnum.Subjects:
                    if (await db.Subjects.AnyAsync())
                        activities = await db.Subjects
                             .Include(s => s.Activities)
                                .ThenInclude(a => a.Room)
                             .Include(s => s.Activities)
                                .ThenInclude(a => a.Subject)
                             .Include(s => s.Activities)
                                .ThenInclude(a => a.ClassGroup)
                             .Where(s => s.Id == entityId)
                             .Select(s => s.Activities)
                             .SingleAsync();
                    break;
                case OptionEnum.Teachers:
                    if (await db.Teachers.AnyAsync())
                        activities = await db.Teachers
                            .Include(t => t.Activities)
                                .ThenInclude(a => a.Room)
                             .Include(t => t.Activities)
                                .ThenInclude(a => a.Subject)
                             .Include(t => t.Activities)
                                .ThenInclude(a => a.ClassGroup)
                            .Where(t => t.Id == entityId)
                            .Select(t => t.Activities)
                            .SingleAsync();
                    break;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}