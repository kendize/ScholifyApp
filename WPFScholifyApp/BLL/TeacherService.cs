// <copyright file="TeacherService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.BLL
{
    using System;
    using System.Linq;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    public class TeacherService
    {
        private IGenericRepository<User> userRepository;

        public TeacherService(IGenericRepository<User> userRepos)
        {
            this.userRepository = userRepos;
        }

        public User AddTeacher(User user)
        {
            this.userRepository.Insert(user);
            this.userRepository.Save();
            return user;
        }

        public void DeleteUser(int userId)
        {
            this.userRepository.Delete(userId);
            this.userRepository.Save();
        }
    }
}
