namespace SchoolScheduler.Models
{
    public class Room : Entity
    {
        public Room() { }
        public Room(Entity entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Activities = entity.Activities;
        }

    }
}