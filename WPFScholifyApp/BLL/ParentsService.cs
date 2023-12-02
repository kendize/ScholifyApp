// <copyright file="ParentsService.cs" company="PlaceholderCompany">
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

    public class ParentsService
    {
        private GenericRepository<User> userRepository;
        private GenericRepository<Pupil> pupilRepository;
        private GenericRepository<Parents> parentsRepository;


        public ParentsService(GenericRepository<User> userRepos, GenericRepository<Pupil> pupilRepos, GenericRepository<Parents> parentsRepos)
        {
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
            this.parentsRepository = parentsRepos;
        }
 
        public List<User> GetParentsForPupilId (int pupilId )
        {
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var allParentsOfPupil = this.userRepository.GetAllq()
                .Include(x => x.Parents)
                .ThenInclude(x => x.ParentsPupils)
                .Where(x => x.Parents!.ParentsPupils!.Select(y => y.pupilId).Contains(pupilId)).ToList();

    //        var allParentsOfPupil = this.parentsRepository
    //.GetAllq()
    //.Where(x => x.ParentsPupils!.Any(y => y.pupilId == pupilId))
    //.Include(x => x.ParentsPupils)
    //    .ThenInclude(pp => pp.pupil!)
    //.Include(x => x.User)
    //.ToList();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            return allParentsOfPupil;
        }

        public void AddParentToPupilId(Parents parents, int pupilId )
        {

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
    }
}
