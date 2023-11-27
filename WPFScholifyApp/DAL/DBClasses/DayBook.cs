// <copyright file="DayBook.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DayBook
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PupilId {  get; set; }

        [ForeignKey("PupilId")]
        public Pupil? Pupil { get; set; }

        public int? Grade { get; set; }

        public string? Attendance { get; set; }

        //[Required]
        public int ScheduleId { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual Schedule? Schedule { get; set; }


    }
}
