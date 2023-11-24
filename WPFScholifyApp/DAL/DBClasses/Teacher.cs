// <copyright file="Teacher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

        public virtual ICollection<DayBook> DayBooks { get; set; } = new List<DayBook>();
    }
}
