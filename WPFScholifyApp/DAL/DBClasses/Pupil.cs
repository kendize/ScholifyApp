﻿namespace WPFScholifyApp.DAL.DBClasses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pupil
    {
        [ForeignKey("User")]
        public int Id { get; set; }

        public virtual User? User { get; set; }

        public int ClassId { get; set; }

        public virtual Class? Class { get; set; }

        public virtual ICollection<Parents>? Parents { get; set; }
    }
}
