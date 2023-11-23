﻿// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.DBClasses
{
    using System;

    public class User
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Role { get; set; }

        public virtual Admin? Admin { get; set; }

        public virtual Teacher? Teacher { get; set; }

        public virtual Pupil? Pupil { get; set; }

        public virtual Parents? Parents { get; set; }
    }
}
