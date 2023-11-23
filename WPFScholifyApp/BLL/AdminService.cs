// <copyright file="AdminService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Windows.Documents;
    using Microsoft.EntityFrameworkCore;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    public class AdminService
    {
        private IGenericRepository<User> userRepository;

        private IGenericRepository<Class> classRepository;

        private IGenericRepository<Teacher> teacherRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<Admin> adminRepository;
        private IGenericRepository<Parents> parentsRepository;

        public AdminService(IGenericRepository<User> userRepos, IGenericRepository<Class> classRepository, IGenericRepository<Teacher> teacherRepository, IGenericRepository<Pupil> pupilRepository, IGenericRepository<Admin> adminRepository, IGenericRepository<Parents> parentsRepository)
        {
            this.userRepository = userRepos;
            this.classRepository = classRepository;
            this.teacherRepository = teacherRepository;
            this.parentsRepository = parentsRepository;
            this.pupilRepository = pupilRepository;
            this.adminRepository = adminRepository;
        }

        public List<Class> GetAllClasses()
        {
            var classes = this.classRepository.GetAll().ToList();
            return classes;
        }

        public List<User> GetAllAdmins()
        {
            var admines = this.userRepository.GetAll().Where(x => x.Role == "адмін").ToList();
            return admines;
        }

        public List<User> GetAllTeacher()
        {
            var teacher = this.userRepository.GetAll().Where(x => x.Role == "вчитель").ToList();
            return teacher;
        }

        public List<User> GetAllParents()
        {
            var parents = this.userRepository.GetAll().Where(x => x.Role == "батьки").ToList();
            return parents;
        }

        public List<User?> GetAllPupilsForClass(int classId)
        {
            var pupilsWithUsers = this.pupilRepository.GetAllq()
            .Include(x => x.User!)
            .Where(x => x.Class!.Id == classId && x.User != null)
            .ToList();

            var userPupils = pupilsWithUsers.Select(pupil => pupil.User).ToList();
            return userPupils;
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
