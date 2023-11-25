// <copyright file="Class.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System.Collections.Generic;

    public class Class
    {
        public int Id { get; set; }

        public string? ClassName { get; set; }

        public virtual ICollection<Advertisement>? Advertisements { get; set; }

        public virtual ICollection<Pupil>? Pupils { get; set; }

        public virtual ICollection<Teacher>? Teachers { get; set; }

        public virtual ICollection<DayBook>? DayBooks { get; set; }

        public virtual ICollection<Schedule>? Schedules { get; set; }
    }
}
