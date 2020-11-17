using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class Teacher : Entity
    {
        public Teacher() { }
        public Teacher(Entity entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Activities = entity.Activities;
        }
    }
}