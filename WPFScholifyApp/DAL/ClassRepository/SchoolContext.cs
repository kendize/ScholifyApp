// <copyright file="SchoolContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.ClassRepository
{
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
        {EntityFrameworkDesignServicesBuilder
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

        public DbSet<DayOfWeek>? DayOfWeeks { get; set; }

        public DbSet<LessonTime>? LessonTimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ScholifyDB;Username=postgres;Password=slava123");
        }
    }

    public class SchoolContextFactory : IDesignTimeDbContextFactory<SchoolContext>
    {
        public SchoolContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            optionsBuilder.UseNpgsql("Data Source=ScholifyDB.db");

            return new SchoolContext(optionsBuilder.Options);
        }
    }
}
