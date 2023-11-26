// <copyright file="Schedule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPFScholifyApp.DAL.DBClasses
{
    public class Schedule
    {
        public int Id { get; set; }
        public int TeacherId { get; internal set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher? Teacher { get; set; }
        [Required]
        public int ClassId { get; internal set; }

        [ForeignKey("ClassId")]
        public virtual Class? Class { get; set; }

        [Required]
        public int DayOfWeekId { get; set; }
        [ForeignKey("DayOfWeekId")]
        public virtual DayOfWeek? DayOfWeek { get; set; }

        [Required]
        public int LessonTimeId { get; set; }
        [ForeignKey("LessonTimeId")]
        public virtual LessonTime? LessonTime { get; set; }

        [Required]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }
    }
}
