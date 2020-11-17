using System.Collections.Generic;

namespace SchoolScheduler.Models
{
    public class ClassGroup : Entity
    {
        public ClassGroup() { }
        public ClassGroup(Entity entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Activities = entity.Activities;
        }
    }
}