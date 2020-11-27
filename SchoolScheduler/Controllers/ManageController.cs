using System.Linq;
using System;
using SchoolScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SchoolScheduler.Controllers
{
    public class ManageController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public ActionResult Index()
        {
            if (!SchoolContext.CanConnect())
                return RedirectToAction("Index", "Error");

            OptionEnum selectedOption = OptionEnum.Rooms;
            if (TempData["selected"] != null)
            {
                selectedOption = (OptionEnum)TempData["selected"];
            }

            OptionList optionList = new OptionList();

            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                    optionList.entities = db.Rooms.Cast<Entity>().ToList();
                    break;
                case OptionEnum.ClassGroups:
                    optionList.entities = db.ClassGroups.Cast<Entity>().ToList();
                    break;
                case OptionEnum.Subjects:
                    optionList.entities = db.Subjects.Cast<Entity>().ToList();
                    break;
                case OptionEnum.Teachers:
                    optionList.entities = db.Teachers.Cast<Entity>().ToList();
                    break;
            }
            optionList.selectedOption = selectedOption;
            ViewData["optionList"] = optionList;
            ViewBag.optionList = optionList;

            return View();
        }

        [HttpPost]
        public ActionResult SelectOption(OptionEnum selectedOption)
        {
            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(OptionEnum selectedOption, int id)
        {
            Entity entityToDelete;

            switch (selectedOption)
            {
                case OptionEnum.Rooms:
                    entityToDelete = await db.Rooms
                        .Include(r => r.Activities)
                        .Where(r => r.Id == id)
                        .SingleOrDefaultAsync();

                    if (entityToDelete == null)
                        TempData["ConcurrencyAlert"] = @"Entity was already deleted by another user.
                        Your operation was cancelled";
                    else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                        TempData["Alert"] = "The entity is used in an activity";
                    else
                        db.Rooms.Remove((Room)entityToDelete);

                    break;
                case OptionEnum.ClassGroups:
                    entityToDelete = await db.ClassGroups
                        .Include(cg => cg.Activities)
                        .Where(cg => cg.Id == id)
                        .SingleAsync();

                    if (entityToDelete == null)
                        TempData["ConcurrencyAlert"] = @"Entity was already deleted by another user.
                        Your operation was cancelled";
                    else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                        TempData["Alert"] = "The entity is used in an activity";
                    else
                        db.ClassGroups.Remove((ClassGroup)entityToDelete);

                    break;
                case OptionEnum.Subjects:
                    entityToDelete = await db.Subjects
                        .Include(s => s.Activities)
                        .Where(s => s.Id == id)
                        .SingleAsync();

                    if (entityToDelete == null)
                        TempData["ConcurrencyAlert"] = @"Entity was already deleted by another user.
                        Your operation was cancelled";
                    else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                        TempData["Alert"] = "The entity is used in an activity";
                    else
                        db.Subjects.Remove((Subject)entityToDelete);

                    break;
                case OptionEnum.Teachers:
                    entityToDelete = await db.Teachers
                        .Include(t => t.Activities)
                        .Where(t => t.Id == id)
                        .SingleAsync();

                    if (entityToDelete == null)
                        TempData["ConcurrencyAlert"] = @"Entity was already deleted by another user.
                        Your operation was cancelled";
                    else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                        TempData["Alert"] = "The entity is used in an activity";
                    else
                        db.Teachers.Remove((Teacher)entityToDelete);

                    break;
            }
            await db.SaveChangesAsync();

            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Add(OptionEnum selectedOption, Entity entity)
        {
            if (entity.Name != null)
            {
                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                        Room newRoom = new Room(entity);
                        if (await db.Rooms.AnyAsync(r => r.Name == newRoom.Name))
                            TempData["Alert"] = "Entity your are trying to add already exists";
                        else
                            await db.Rooms.AddAsync(newRoom);
                        break;
                    case OptionEnum.ClassGroups:
                        ClassGroup newClassGroup = new ClassGroup(entity);
                        if (await db.ClassGroups.AnyAsync(cg => cg.Name == newClassGroup.Name))
                            TempData["Alert"] = "Entity your are trying to add already exists";
                        else
                            await db.ClassGroups.AddAsync(newClassGroup);
                        break;
                    case OptionEnum.Subjects:
                        Subject newSubject = new Subject(entity);
                        if (await db.Subjects.AnyAsync(s => s.Name == newSubject.Name))
                            TempData["Alert"] = "Entity your are trying to add already exists";
                        else
                            await db.Subjects.AddAsync(newSubject);
                        break;
                    case OptionEnum.Teachers:
                        Teacher newTeacher = new Teacher(entity);
                        if (await db.Teachers.AnyAsync(t => t.Name == newTeacher.Name))
                            TempData["Alert"] = "Entity your are trying to add already exists";
                        else
                            await db.Teachers.AddAsync(newTeacher);
                        break;
                }
                await db.SaveChangesAsync();
            }
            else
                TempData["Alert"] = "Cannot add entity with empty value";
            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
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