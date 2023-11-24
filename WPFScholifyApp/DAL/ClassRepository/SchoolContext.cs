// <copyright file="SchoolContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.ClassRepository
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using WPFScholifyApp.DAL.DBClasses;

    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public SchoolContext()
        {
            this.Database.EnsureCreated();
        }

        public DbSet<User>? Users { get; set; }

        public DbSet<Admin>? Admins { get; set; }

        public DbSet<Teacher>? Teachers { get; set; }

        public DbSet<Pupil>? Pupils { get; set; }

        public DbSet<Parents>? Parents { get; set; }

        public DbSet<Class>? Classes { get; set; }

        public DbSet<Schedule>? Schedules { get; set; }

        public DbSet<DayBook>? DayBooks { get; set; }

        public DbSet<Subject>? Subjects { get; set; }

        public DbSet<DBClasses.DayOfWeek>? DayOfWeeks { get; set; }

        public DbSet<LessonTime>? LessonTimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ScholifyDBTest;Username=postgres;Password=1234");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
    .HasIndex(x => x.UserId)
    .HasDatabaseName("IX_Teachers_UserId"); // Provide the name of the index to remove

            var indexForRemoval = modelBuilder.Entity<Teacher>().Metadata.FindIndex("IX_Teachers_UserId");
            if (indexForRemoval != null)
            {
                modelBuilder.Entity<Teacher>().Metadata.RemoveIndex(indexForRemoval);
            }

            // Додаємо тестові дані при створені бази даних
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "1", Password = "1", FirstName = "AdminName", LastName = "AdminLastname", Role = "адмін" },
                new User { Id = 2, Email = "user1@example.com", Password = "password1", FirstName = "John1", LastName = "Teacher", Role = "вчитель" },
                new User { Id = 3, Email = "user2@example.com", Password = "password1", FirstName = "John2", LastName = "Doe2", Role = "вчитель" },
                new User { Id = 4, Email = "user3@example.com", Password = "password1", FirstName = "John3", LastName = "Doe3", Role = "вчитель" });

            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1 });

            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, UserId = 2, SubjectId = 1 });

            modelBuilder.Entity<Pupil>().HasData(
                new Pupil { Id = 1, ClassId = 1 });

            modelBuilder.Entity<Parents>().HasData(
                new Parents { Id = 1 });

            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, ClassName = "Class A" });

            modelBuilder.Entity<Schedule>().HasData(
                new Schedule { Id = 1, TeacherId = 1, ClassId = 1 });

            modelBuilder.Entity<DayBook>().HasData(
                new DayBook { Id = 1, Grade = 10, Attendance = "Present", Date = DateTime.UtcNow, ClassId = 1, TeacherId = 1 });

            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, SubjectName = "Math" });

            modelBuilder.Entity<DBClasses.DayOfWeek>().HasData(
                new DBClasses.DayOfWeek { Id = 1, Day = "Monday" });

            modelBuilder.Entity<LessonTime>().HasData(
                new LessonTime { Id = 1, Start = "9:00 AM", End = "10:00 AM" });
        }
    }

    public class SchoolContextFactory : IDesignTimeDbContextFactory<SchoolContext>
    {
        public SchoolContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            optionsBuilder.UseNpgsql("Data Source=ScholifyDBTest.db");

            return new SchoolContext(optionsBuilder.Options);
        }
    }
}
