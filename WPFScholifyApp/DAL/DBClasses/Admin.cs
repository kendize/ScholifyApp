// <copyright file="Admin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Admin
    {
        [ForeignKey("User")]
        public int Id { get; set; }

        public virtual User? User { get; set; }
    }
}
