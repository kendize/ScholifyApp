// <copyright file="SchoolContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.ClassRepository
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using WPFScholifyApp.DAL.DBClasses;
    using WPFScholifyApp.Enums;

    public class SchoolContext : DbContext
    {
        private readonly Times times = new Times();
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

        public DbSet<Advertisement> Advertisements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ScholifyDBTest;Username=postgres;Password=1234");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
            .HasIndex(x => x.UserId)
            .IsUnique(false);

            modelBuilder.Entity<ParentsPupil>()
                .HasKey(pt => new {pt.pupilId, pt.parentId});

            modelBuilder.Entity<ParentsPupil>()
                .HasOne(x => x.parent)
                .WithMany(x => x.ParentsPupils)
                .HasForeignKey(x => x.parentId);

            modelBuilder.Entity<ParentsPupil>()
                .HasOne(x => x.pupil)
                .WithMany(x => x.ParentsPupil)
                .HasForeignKey(x => x.pupilId);

            // Додаємо тестові дані при створені бази даних
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "1", Password = "1", FirstName = "Адміністратор",LastName = "Платформи",     MiddleName = "",                Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "адмін",    Address = "м.Житомир,вул Перемоги, 1" },
                new User { Id = 2, Email = "2", Password = "2", FirstName = "Марія",        LastName = "Галушко",       MiddleName = "Вікторівна",      Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "вчитель" , Address = "м.Житомир,вул Перемоги, 2" },
                new User { Id = 3, Email = "3", Password = "3", FirstName = "Ігор",         LastName = "Гнатюк",        MiddleName = "Іванович",        Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "вчитель",  Address = "м.Житомир,вул Перемоги, 3" },
                new User { Id = 4, Email = "4", Password = "4", FirstName = "Олег",         LastName = "Винник",        MiddleName = "Павлович",        Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "вчитель",  Address = "м.Житомир,вул Перемоги, 4" },
                new User { Id = 5, Email = "5", Password = "5", FirstName = "Михайло",      LastName = "Іващенко",      MiddleName = "Святославович",   Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Gender = "чоловік",Address = "м.Житомир,вул Перемоги, 5", PhoneNumber = "0685495126", Role = "учень" },
                new User { Id = 6, Email = "6", Password = "6", FirstName = "Світлана",     LastName = "Романюк",       MiddleName = "Василівна",       Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Gender = "жінка",  Address = "м.Харків вул.Миру,8", PhoneNumber ="0635472856", Role = "батьки"});
            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1 });

            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, UserId = 2, SubjectId = 1 });

            modelBuilder.Entity<Pupil>().HasData(
                new Pupil { Id = 5, UserId = 5, ClassId = 1 });

            modelBuilder.Entity<Parents>().HasData(
                new Parents { Id = 6, UserId = 6 });

            modelBuilder.Entity<ParentsPupil>().HasData(
                new ParentsPupil { parentId = 6, pupilId = 5 });

            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, ClassName = "11-А" });

            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, SubjectName = "Математика", ClassId = 1 });
            
            modelBuilder.Entity<DBClasses.DayOfWeek>().HasData(
                new DBClasses.DayOfWeek { Id = 1, Day = "27.11.2023", Date = new DateTime(2023, 11, 27).ToUniversalTime() });

            modelBuilder.Entity<LessonTime>().HasData(
                new LessonTime { Id = 1, Start = Times.t8_30.ToString("HH:mm"), End = Times.t9_15.ToString("HH:mm"), StartTime = Times.t8_30, EndTime = Times.t9_15 },
                new LessonTime { Id = 2, Start = Times.t9_30.ToString("HH:mm"), End = Times.t10_15.ToString("HH:mm"), StartTime = Times.t9_30, EndTime = Times.t10_15 },
                new LessonTime { Id = 3, Start = Times.t10_30.ToString("HH:mm"), End = Times.t11_15.ToString("HH:mm"), StartTime = Times.t10_30, EndTime = Times.t11_15 },
                new LessonTime { Id = 4, Start = Times.t11_35.ToString("HH:mm"), End = Times.t12_20.ToString("HH:mm"), StartTime = Times.t11_35, EndTime = Times.t12_20 },
                new LessonTime { Id = 5, Start = Times.t12_40.ToString("HH:mm"), End = Times.t13_25.ToString("HH:mm"), StartTime = Times.t12_40, EndTime = Times.t13_25 },
                new LessonTime { Id = 6, Start = Times.t13_35.ToString("HH:mm"), End = Times.t14_20.ToString("HH:mm"), StartTime = Times.t13_35, EndTime = Times.t14_20 },
                new LessonTime { Id = 7, Start = Times.t14_35.ToString("HH:mm"), End = Times.t15_20.ToString("HH:mm"), StartTime = Times.t14_35, EndTime = Times.t15_20 },
                new LessonTime { Id = 8, Start = Times.t15_30.ToString("HH:mm"), End = Times.t16_15.ToString("HH:mm"), StartTime = Times.t15_30, EndTime = Times.t16_15 });

            modelBuilder.Entity<Schedule>().HasData(
                new Schedule { Id = 1, TeacherId = 1, ClassId = 1, LessonTimeId = 1, DayOfWeekId = 1, SubjectId = 1 });

            modelBuilder.Entity<DayBook>().HasData(
                new DayBook { Id = 1, Grade = 10, PupilId = 5, Attendance = "Present", ScheduleId = 1 });
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
