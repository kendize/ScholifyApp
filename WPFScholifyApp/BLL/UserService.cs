// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.BLL
{
    using System;
    using System.Linq;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    public class UserService
    {
        private IGenericRepository<User> userRepository;

        public UserService(IGenericRepository<User> userRepos)
        {
            this.userRepository = userRepos;
        }

        public User Authenticate(string email, string password, string role)
        {
            var user = this.userRepository.GetAll().FirstOrDefault(u => u.Email == email && u.Password == password && u.Role == role);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new InvalidOperationException("Entity not found");
            }
        }

        public User AuthenticateEmail(string email)
        {
            var user = this.userRepository.GetAll().FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new InvalidOperationException("Entity not found");
            }
        }

        public User AuthenticatePassword(string password)
        {
            var user = this.userRepository.GetAll().FirstOrDefault(u => u.Password == password);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new InvalidOperationException("Entity not found");
            }
        }

        public User GetInfoByNameSurname(string name, string surname)
        {
            var user = this.userRepository.GetAll().FirstOrDefault(u => u.FirstName == name && u.LastName == surname);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new InvalidOperationException("Entity not found");
            }
        }
    }
}
