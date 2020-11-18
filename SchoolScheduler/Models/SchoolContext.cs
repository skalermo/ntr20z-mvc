using Microsoft.EntityFrameworkCore;
using System;
using SchoolScheduler.Models;
public class SchoolContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<ClassGroup> ClassGroups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Slot> Slots { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(
            "Server = hparch; Database = NTR20Z; User Id = admin; Password = admin"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        InitialMigration(modelBuilder);
        PopulateMigration(modelBuilder);
    }

    private void InitialMigration(ModelBuilder mb)
    {
        string[] days = { "Mo", "Tu", "We", "Th", "Fr" };
        string[] times = { "8:00-8:45", "8:55-9:40", "9:50-11:35",
        "11:55-12:40", "12:50-13:55", "13:45-14:30",
        "14:40-15:25", "15:35-16:20", "16:30-17:15" };

        int i = 1;
        foreach (var time in times)
        {
            foreach (var day in days)
            {
                mb.Entity<Slot>().HasData(new Slot() { SlotId = i++, Name = day + " " + time });
            }
        }
    }

    private void PopulateMigration(ModelBuilder mb)
    {
        string[] roomNames = { "110", "111", "120", "121" };
        int i = 1;
        foreach (var name in roomNames)
        {
            mb.Entity<Room>().HasData(new Room() { Id = i++, Name = name });
        }

        string[] classGroupNames = { "1a", "1b", "1c", "2a", "2b", "3a", "3b", "4a", "4b" };
        i = 1;
        foreach (var name in classGroupNames)
        {
            mb.Entity<ClassGroup>().HasData(new ClassGroup() { Id = i++, Name = name });
        }

        string[] subjectNames = { "mat", "geo", "eng", "phys", "biol" };
        i = 1;
        foreach (var name in subjectNames)
        {
            mb.Entity<Subject>().HasData(new Subject() { Id = i++, Name = name });
        }

        string[] teacherNames = { "kowalski", "nowak", "smith", "clarkson", "may", "hammond", "atkinson" };
        i = 1;
        foreach (var name in teacherNames)
        {
            mb.Entity<Teacher>().HasData(new Teacher() { Id = i++, Name = name });
        }

        mb.Entity<Activity>().HasData(new Activity() { ActivityId = 1, RoomId = 1, ClassGroupId = 1, SubjectId = 1, SlotId = 1, TeacherId = 1 });
        mb.Entity<Activity>().HasData(new Activity() { ActivityId = 2, RoomId = 4, ClassGroupId = 3, SubjectId = 3, SlotId = 3, TeacherId = 2 });
    }
}