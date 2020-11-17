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


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(
            "Server = hparch; Database = NTR20Z; User Id = admin; Password = admin"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
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
                modelBuilder.Entity<Slot>().HasData(new Slot() { SlotId = i++, Name = day + " " + time });
            }
        }
    }
}