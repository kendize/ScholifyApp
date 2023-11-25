// <copyright file="TeacherService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.BLL
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    public class TeacherService
    {
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Advertisement> advertisementRepository;
        private IGenericRepository<Class> classRepository;

        public TeacherService(IGenericRepository<User> userRepos, IGenericRepository<Advertisement> advertisementRepository, IGenericRepository<Class> classRepository)
        {
            this.userRepository = userRepos;
            this.advertisementRepository = advertisementRepository;
            this.classRepository = classRepository;
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


        public List<Advertisement> GetAllAdvertisementsForClassId(int classId)
        {
            var advertisementsForClass =  this.advertisementRepository.GetAllq()
                .Include(a => a.Class)
                .Where(a => a.ClassId == classId).ToList();

            return advertisementsForClass!;
        }

    }
}
