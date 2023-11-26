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
        private IGenericRepository<Advertisement> advertisementRepository;
        private IGenericRepository<Class> classRepository;

        private IGenericRepository<Teacher> teacherRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<Admin> adminRepository;
        private IGenericRepository<Parents> parentsRepository;
        private IGenericRepository<Subject> subjectRepository;
        private IGenericRepository<Schedule> scheduleRepository;
        private IGenericRepository<User> userRepository;

        public TeacherService(IGenericRepository<Advertisement> advertisementRepository, IGenericRepository<User> userRepos, IGenericRepository<Class> classRepository, IGenericRepository<Teacher> teacherRepository, IGenericRepository<Pupil> pupilRepository, IGenericRepository<Admin> adminRepository, IGenericRepository<Parents> parentsRepository, IGenericRepository<Subject> subjectRepository, IGenericRepository<Schedule> scheduleRepository)
        {
            this.advertisementRepository = advertisementRepository;
            this.userRepository = userRepos;
            this.classRepository = classRepository;
            this.teacherRepository = teacherRepository;
            this.parentsRepository = parentsRepository;
            this.pupilRepository = pupilRepository;
            this.adminRepository = adminRepository;
            this.subjectRepository = subjectRepository;
            this.scheduleRepository = scheduleRepository;
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

        public List<Schedule> GetAllSchedules(int teacherId, int dayOfWeek)
        {
            return this.scheduleRepository.GetAllq()
                .Include(x => x.DayOfWeek)
                .Include(x => x.Class)
                .Include(x => x.Teacher)
                .Include(x => x.LessonTime)
                .Include(x => x.Subject)
                .Where(x => x.Teacher!.Id == teacherId && x.DayOfWeekId == dayOfWeek).ToList();
        }

    }
}
