namespace SchoolScheduler.Models
{
    public class Subject : Entity
    {
        public Subject() { }
        public Subject(Entity entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Activities = entity.Activities;
        }
    }
}