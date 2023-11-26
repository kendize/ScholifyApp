// <copyright file="DayOfWeek.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System;
    using System.Collections.Generic;

    public class DayOfWeek
    {
        public int Id { get; set; }

        public string? Day { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Schedule>? Schedules { get; set; }
    }
}
