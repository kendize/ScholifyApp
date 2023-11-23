// <copyright file="DayBook.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System;

    public class DayBook
    {
        public int Id { get; set; }

        public int? Grade { get; set; }

        public string? Attendance { get; set; }

        public DateTime? Date { get; set; }

        public virtual Class? Class { get; set; }

        public virtual Teacher? Teacher { get; set; }

        public virtual Subject? Subject { get; set; }
    }
}
