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
        private GenericRepository<Advertisement> advertisementRepository;
        private GenericRepository<Teacher> teacherRepository;
        private GenericRepository<Subject> subjectRepository;
        private GenericRepository<Schedule> scheduleRepository;
        private GenericRepository<User> userRepository;

        public TeacherService(GenericRepository<Advertisement> advertisementRepository, GenericRepository<User> userRepos, GenericRepository<Teacher> teacherRepository, GenericRepository<Subject> subjectRepository, GenericRepository<Schedule> scheduleRepository)
        {
            this.advertisementRepository = advertisementRepository;
            this.userRepository = userRepos;
            this.teacherRepository = teacherRepository;
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

        public List<Subject> ShowAllSubjectsForTeacherId (int teacherId)
        {
            var subjects = this.subjectRepository.GetAllq()
                .Include(x => x.Teachers)
                .Include(x => x.Class)
                .Where(x => x.Teachers!.Select(y => y.UserId).Contains(teacherId)).ToList();

            return subjects;
        }

        public List<int> GetAllTeacherIds()
        {
            return this.teacherRepository.GetAll().Select(t => t.Id).ToList();
        }

        public Teacher GetTeacherBySubjectId(int subjectId)
        {
            return this.teacherRepository.GetAll().FirstOrDefault(x => x.SubjectId == subjectId)!;
        }

        public void Delete(int teacherId)
        {
            this.teacherRepository.Delete(teacherId);

        }
    }
}
