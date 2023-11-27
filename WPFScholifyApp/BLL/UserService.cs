// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.BLL
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    public class UserService
    {
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<Parents> parentRepository;
        private IGenericRepository<ParentsPupil> parentsPupilRepository;

        public UserService(IGenericRepository<User> userRepos, IGenericRepository<Pupil> pupilRepos, IGenericRepository<Parents> parentRepository, IGenericRepository<ParentsPupil> parentsPupilRepository)
        {
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
            this.parentRepository = parentRepository;
            this.parentsPupilRepository = parentsPupilRepository;
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

        public void DeleteParent(int parentId)
        {
            var parent = this.parentRepository.GetAllq().Include(x => x.ParentsPupils).FirstOrDefault(x => x.Id == parentId);

            this.parentRepository.Delete(parentId);
            this.parentRepository.Save();

            this.userRepository.Delete(parent.UserId);
            this.userRepository.Save();
        }
    }
}
