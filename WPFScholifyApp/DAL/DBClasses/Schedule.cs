// <copyright file="Schedule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    public class Schedule
    {
        public int Id { get; set; }

        public virtual Teacher? Teacher { get; set; }

        public virtual Class? Class { get; set; }

        public virtual DayOfWeek? DayOfWeek { get; set; }

        public virtual LessonTime? LessonTime { get; set; }

        public virtual Subject? Subject { get; set; }
    }
}
