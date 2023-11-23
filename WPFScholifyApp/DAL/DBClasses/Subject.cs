// <copyright file="Subject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System.Collections.Generic;

    public class Subject
    {
        public int Id { get; set; }

        public string? SubjectName { get; set; }

        public virtual ICollection<DayBook>? DayBooks { get; set; }

        public virtual ICollection<Teacher>? Teachers { get; set; }

        public virtual ICollection<Schedule>? Schedules { get; set; }
    }
}
