// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.BLL
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    public class UserService
    {
        private GenericRepository<User> userRepository;
        private GenericRepository<Pupil> pupilRepository;
        private GenericRepository<Parents> parentRepository;
        private GenericRepository<ParentsPupil> parentsPupilRepository;

        public UserService(GenericRepository<User> userRepos, GenericRepository<Pupil> pupilRepos, GenericRepository<Parents> parentRepository, GenericRepository<ParentsPupil> parentsPupilRepository)
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

        public User AddUser(User user)
        {
            this.userRepository.Insert(user);
            this.userRepository.Save();
            return user;
        }

        public User AddUser(User user, Pupil pupil)
        {
            this.userRepository.Insert(user);
            this.pupilRepository.Insert(pupil);
            this.userRepository.Save();
            this.pupilRepository.Save();
            return user;
        }

        public User AddUser(User user, Parents parents)
        {
                this.userRepository.Insert(user);
                this.parentRepository.Insert(parents);
                this.userRepository.Save();
                this.pupilRepository.Save();
                var ParentsPupil = new ParentsPupil
                {
                    pupilId = user!.Id,
                    parentId = parents.Id
                };

                this.parentsPupilRepository.Insert(ParentsPupil);
                this.parentsPupilRepository.Save();
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

        public List<User> ShowUsersForSubjectId(int subjectId)
        {
            var group = this.userRepository.GetAllq()
                .Include(x => x.Pupil)
                .ThenInclude(x => x.Class)
                .ThenInclude(x => x.Subjects)
                .Where(x => x.Pupil.Class.Subjects!.Select(y => y.Id).Contains(subjectId)).ToList();

            return group;
        }

        public void SaveUser(User user)
        {
            this.userRepository.Update(user);
            this.userRepository.Save();
        }

        public User GetUserById(int id)
        {
            return this.userRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
        }
    }
}
