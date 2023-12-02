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
    using DayOfWeek = DBClasses.DayOfWeek;

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
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ScholifyDBV3;Username=postgres;Password=1234");
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
                .HasForeignKey(x => x.parentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ParentsPupil>()
                .HasOne(x => x.pupil)
                .WithMany(x => x.ParentsPupil)
                .HasForeignKey(x => x.pupilId)
                .OnDelete(DeleteBehavior.Cascade);

            // Додаємо тестові дані при створені бази даних
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "1", Password = "1", FirstName = "Адміністратор",LastName = "Платформи",     MiddleName = "",                Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "адмін",    Address = "м.Житомир,вул Перемоги, 1" },
                new User { Id = 2, Email = "2", Password = "2", FirstName = "Марія",        LastName = "Галушко",       MiddleName = "Вікторівна",      Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "вчитель" , Address = "м.Житомир,вул Перемоги, 2" },
                new User { Id = 3, Email = "3", Password = "3", FirstName = "Ігор",         LastName = "Гнатюк",        MiddleName = "Іванович",        Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "вчитель",  Address = "м.Житомир,вул Перемоги, 3" },
                new User { Id = 4, Email = "4", Password = "4", FirstName = "Олег",         LastName = "Винник",        MiddleName = "Павлович",        Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "вчитель",  Address = "м.Житомир,вул Перемоги, 4" },
                new User { Id = 5, Email = "5", Password = "5", FirstName = "Михайло",      LastName = "Іващенко",      MiddleName = "Святославович",   Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Gender = "чоловік",Address = "м.Житомир,вул Перемоги, 5", PhoneNumber = "0685495126", Role = "учень" },
                new User { Id = 6, Email = "6", Password = "6", FirstName = "Світлана",     LastName = "Романюк",       MiddleName = "Василівна",       Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Gender = "жінка",  Address = "м.Харків вул.Миру,8", PhoneNumber ="0635472856", Role = "батьки"},
                new User { Id = 12, Email = "12", Password = "12", FirstName = "Іван", LastName = "Іванов", MiddleName = "Іванович", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "учень", Address = "м.Житомир,вул Перемоги, 12" },
                new User { Id = 13, Email = "13", Password = "13", FirstName = "Олена", LastName = "Петренко", MiddleName = "Володимирівна", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "учень", Address = "м.Житомир,вул Перемоги, 13" },
                new User { Id = 14, Email = "14", Password = "14", FirstName = "Максим", LastName = "Семенов", MiddleName = "Віталійович", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "учень", Address = "м.Житомир,вул Перемоги, 14" },
                new User { Id = 15, Email = "15", Password = "15", FirstName = "Юлія", LastName = "Горбач", MiddleName = "Ігорівна", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "учень", Address = "м.Житомир,вул Перемоги, 15" },
                new User { Id = 16, Email = "16", Password = "16", FirstName = "Сергій", LastName = "Лисенко", MiddleName = "Олександрович", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "учень", Address = "м.Житомир,вул Перемоги, 16" },
                new User { Id = 17, Email = "17", Password = "17", FirstName = "Марина", LastName = "Данилюк", MiddleName = "Василівна", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "батьки", Address = "м.Житомир,вул Перемоги, 17" },
                new User { Id = 18, Email = "18", Password = "18", FirstName = "Олександр", LastName = "Коваль", MiddleName = "Михайлович", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "батьки", Address = "м.Житомир,вул Перемоги, 18" },
                new User { Id = 19, Email = "19", Password = "19", FirstName = "Тетяна", LastName = "Мельник", MiddleName = "Андріївна", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "батьки", Address = "м.Житомир,вул Перемоги, 19" },
                new User { Id = 20, Email = "20", Password = "20", FirstName = "Ігор", LastName = "Білецький", MiddleName = "Петрович", Birthday = new DateTime(2023, 11, 25).ToUniversalTime(), Role = "батьки", Address = "м.Житомир,вул Перемоги, 20" });
            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1 },
                new Admin { Id = 3 },
                new Admin { Id = 4 });

            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, ClassName = "11-А" },
                new Class { Id = 2, ClassName = "11-Б" },
                new Class { Id = 3, ClassName = "11-В" },
                new Class { Id = 4, ClassName = "11-Г" },
                new Class { Id = 5, ClassName = "11-Д" });
            modelBuilder.Entity<DayOfWeek>().HasData(
                new DayOfWeek { Id = 2, Day = "28.11.2023", Date = new DateTime(2023, 11, 28).ToUniversalTime() },
                new DayOfWeek { Id = 3, Day = "29.11.2023", Date = new DateTime(2023, 11, 29).ToUniversalTime() },
                new DayOfWeek { Id = 4, Day = "30.11.2023", Date = new DateTime(2023, 11, 30).ToUniversalTime() },
                new DayOfWeek { Id = 5, Day = "01.12.2023", Date = new DateTime(2023, 12, 1).ToUniversalTime() },
                new DayOfWeek { Id = 6, Day = "02.12.2023", Date = new DateTime(2023, 12, 2).ToUniversalTime() },
                new DayOfWeek { Id = 7, Day = "03.12.2023", Date = new DateTime(2023, 12, 3).ToUniversalTime() },
                new DayOfWeek { Id = 8, Day = "04.12.2023", Date = new DateTime(2023, 12, 4).ToUniversalTime() },
                new DayOfWeek { Id = 9, Day = "05.12.2023", Date = new DateTime(2023, 12, 5).ToUniversalTime() },
                new DayOfWeek { Id = 10, Day = "06.12.2023", Date = new DateTime(2023, 12, 6).ToUniversalTime() },
                new DayOfWeek { Id = 11, Day = "07.12.2023", Date = new DateTime(2023, 12, 7).ToUniversalTime() }
);

            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, SubjectName = "Математика", ClassId = 1 },
                new Subject { Id = 2, SubjectName = "Фізика", ClassId = 2 },
                new Subject { Id = 3, SubjectName = "Географія", ClassId = 3 },
                new Subject { Id = 4, SubjectName = "Історія", ClassId = 4 },
                new Subject { Id = 5, SubjectName = "Укр. Мова", ClassId = 5 },
                new Subject { Id = 6, SubjectName = "Інформатика", ClassId = 5 });

            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, UserId = 2, SubjectId = 1 },
                new Teacher { Id = 7, UserId = 13, SubjectId = 2 },
                new Teacher { Id = 8, UserId = 14, SubjectId = 3 },
                new Teacher { Id = 9, UserId = 15, SubjectId = 4 },
                new Teacher { Id = 10, UserId = 16, SubjectId = 5 },
                new Teacher { Id = 11, UserId = 17, SubjectId = 6 });



            modelBuilder.Entity<Pupil>().HasData(
                new Pupil { Id = 5, UserId = 5, ClassId = 1 },
                new Pupil { Id = 12, UserId = 12, ClassId = 1 },
                new Pupil { Id = 13, UserId = 13, ClassId = 1 },
                new Pupil { Id = 14, UserId = 14, ClassId = 2 },
                new Pupil { Id = 15, UserId = 15, ClassId = 2 },
                new Pupil { Id = 16, UserId = 16, ClassId = 3 },
                new Pupil { Id = 18, UserId = 18, ClassId = 4 });

            modelBuilder.Entity<Parents>().HasData(
                new Parents { Id = 17, UserId = 17  },
                new Parents { Id = 18, UserId = 18  },
                new Parents { Id = 19, UserId = 19 },
                new Parents { Id = 20, UserId = 20 });


            modelBuilder.Entity<ParentsPupil>().HasData(
                new ParentsPupil { parentId = 17, pupilId = 13 },
                new ParentsPupil { parentId = 17, pupilId = 14 },
                new ParentsPupil { parentId = 18, pupilId = 13 },
                new ParentsPupil { parentId = 18, pupilId =14 },
                new ParentsPupil { parentId = 19, pupilId = 15 },
                new ParentsPupil { parentId = 19, pupilId = 16 },
                new ParentsPupil { parentId = 20, pupilId = 5 },
                new ParentsPupil { parentId = 20, pupilId = 16 }

                );

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
                new Schedule { Id = 1, TeacherId = 1, ClassId = 1, LessonTimeId = 1, DayOfWeekId = 1, SubjectId = 1 },
                new Schedule { Id = 8, TeacherId = 7, ClassId = 2, LessonTimeId = 1, DayOfWeekId = 1, SubjectId = 2 },
                new Schedule { Id = 9, TeacherId = 8, ClassId = 3, LessonTimeId = 2, DayOfWeekId = 2, SubjectId = 3 },
                new Schedule { Id = 10, TeacherId = 9, ClassId = 4, LessonTimeId = 3, DayOfWeekId = 3, SubjectId = 4 },
                new Schedule { Id = 11, TeacherId = 10, ClassId = 5, LessonTimeId = 4, DayOfWeekId = 4, SubjectId = 5 });

            modelBuilder.Entity<DayBook>().HasData(
                new DayBook { Id = 1, Grade = 10, PupilId = 5, Attendance = "Present", ScheduleId = 1 },
                new DayBook { Id = 8, Grade = 9, PupilId = 12, Attendance = "Present", ScheduleId = 8 },
                new DayBook { Id = 9, Grade = 8, PupilId = 13, Attendance = "Absent", ScheduleId = 9 },
                new DayBook { Id = 10, Grade = 10, PupilId = 14, Attendance = "Present", ScheduleId = 10 },
                new DayBook { Id = 11, Grade = 7, PupilId = 15, Attendance = "Present", ScheduleId = 11 });
        }
    }

    public class SchoolContextFactory : IDesignTimeDbContextFactory<SchoolContext>
    {
        public SchoolContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            optionsBuilder.UseNpgsql("Data Source=ScholifyDBV3.db");

            return new SchoolContext(optionsBuilder.Options);
        }
    }
}
