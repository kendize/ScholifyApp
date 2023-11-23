// <copyright file="Parents.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Parents
    {
        [ForeignKey("User")]
        public int Id { get; set; }

        public virtual User? User { get; set; }

        public virtual ICollection<Pupil>? Pupils { get; set; }
    }
}
