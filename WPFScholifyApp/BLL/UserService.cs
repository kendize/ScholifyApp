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
        private IGenericRepository<Pupil> pupilRepository;

        public UserService(IGenericRepository<User> userRepos, IGenericRepository<Pupil> pupilRepos)
        {
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
        }

        public User Authenticate(string email, string password)
        {
            var user = this.userRepository.GetAll().FirstOrDefault(u => u.Email == email && u.Password == password);
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

        public User AddUser(User user, Pupil pupil)
        {
            this.userRepository.Insert(user);
            this.pupilRepository.Insert(pupil);
            this.userRepository.Save();
            this.pupilRepository.Save();
            return user;
        }

        public void DeletePupil(int userId)
        {
            this.userRepository.Delete(userId);
            this.userRepository.Save();
        }

        //public void DeletePerents((int userId)
        //{
        //    this.userRepository.Delete(userId);
        //    this.userRepository.Save();
        //}
    }
}
