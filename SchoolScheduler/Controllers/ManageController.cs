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
            using (var context = new SchoolContext())
            {
                Entity entityToDelete;

                switch (selectedOption)
                {
                    case OptionEnum.Rooms:
                        entityToDelete = await context.Rooms
                            .Include(r => r.Activities)
                            .Where(r => r.Id == id)
                            .SingleOrDefaultAsync();

                        if (entityToDelete == null)
                            TempData["Alert"] = "Concurrency warning: already deleted";
                        else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                            TempData["Alert"] = "Used in an Activity";
                        else
                            context.Rooms.Remove((Room)entityToDelete);

                        break;
                    case OptionEnum.ClassGroups:
                        entityToDelete = await context.ClassGroups
                            .Include(cg => cg.Activities)
                            .Where(cg => cg.Id == id)
                            .SingleAsync();

                        if (entityToDelete == null)
                            TempData["Alert"] = "Concurrency warning: already deleted";
                        else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                            TempData["Alert"] = "Used in an Activity";
                        else
                            context.ClassGroups.Remove((ClassGroup)entityToDelete);

                        break;
                    case OptionEnum.Subjects:
                        entityToDelete = await context.Subjects
                            .Include(s => s.Activities)
                            .Where(s => s.Id == id)
                            .SingleAsync();

                        if (entityToDelete == null)
                            TempData["Alert"] = "Concurrency warning: already deleted";
                        else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                            TempData["Alert"] = "Used in an Activity";
                        else
                            context.Subjects.Remove((Subject)entityToDelete);

                        break;
                    case OptionEnum.Teachers:
                        entityToDelete = await context.Teachers
                            .Include(t => t.Activities)
                            .Where(t => t.Id == id)
                            .SingleAsync();

                        if (entityToDelete == null)
                            TempData["Alert"] = "Concurrency warning: already deleted";
                        else if (entityToDelete.Activities != null && entityToDelete.Activities.Any())
                            TempData["Alert"] = "Used in an Activity";
                        else
                            context.Teachers.Remove((Teacher)entityToDelete);

                        break;
                }
                context.SaveChanges();
            }

            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Add(OptionEnum selectedOption, Entity entity)
        {
            if (entity.Name != null)
                using (var context = new SchoolContext())
                {
                    switch (selectedOption)
                    {
                        case OptionEnum.Rooms:
                            Room newRoom = new Room(entity);
                            if (context.Rooms.Any(r => r.Name == newRoom.Name))
                                TempData["Alert"] = "Entity already exists";
                            else
                                context.Rooms.Add(newRoom);
                            break;
                        case OptionEnum.ClassGroups:
                            ClassGroup newClassGroup = new ClassGroup(entity);
                            if (context.ClassGroups.Any(cg => cg.Name == newClassGroup.Name))
                                TempData["Alert"] = "Entity already exists";
                            else
                                context.ClassGroups.Add(newClassGroup);
                            break;
                        case OptionEnum.Subjects:
                            Subject newSubject = new Subject(entity);
                            if (context.Subjects.Any(s => s.Name == newSubject.Name))
                                TempData["Alert"] = "Entity already exists";
                            else
                                context.Subjects.Add(newSubject);
                            break;
                        case OptionEnum.Teachers:
                            Teacher newTeacher = new Teacher(entity);
                            if (context.Teachers.Any(t => t.Name == newTeacher.Name))
                                TempData["Alert"] = "Entity already exists";
                            else
                                context.Teachers.Add(newTeacher);
                            break;
                    }
                    context.SaveChanges();
                }
            else
                TempData["Alert"] = "Cannot add empty name";
            TempData["selected"] = selectedOption;
            return RedirectToAction("Index");
        }
    }
}