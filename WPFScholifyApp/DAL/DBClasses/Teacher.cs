// <copyright file="Teacher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Teacher
    {
        [ForeignKey("User")]
        public int Id { get; set; }

        public virtual User? User { get; set; }

        public virtual Subject? Subject { get; set; }

        public virtual ICollection<Class>? Classes { get; set; }

        public virtual ICollection<Schedule>? Schedules { get; set; }

        public virtual ICollection<DayBook>? DayBooks { get; set; }
    }
}
