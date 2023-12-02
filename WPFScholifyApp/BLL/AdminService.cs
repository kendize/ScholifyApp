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
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    public class AdminService
    {
        private GenericRepository<Advertisement> advertisementsRepository;
        private GenericRepository<Class> classRepository;
        private GenericRepository<DayBook> dayBookRepository;
        private GenericRepository<DayOfWeek> dayOfWeekRepository;
        private GenericRepository<Parents> parentsRepository;
        private GenericRepository<ParentsPupil> parentsPupilRepository;
        private GenericRepository<Pupil> pupilRepository;
        private GenericRepository<Subject> subjectRepository;
        private GenericRepository<Teacher> teacherRepository;
        private GenericRepository<User> userRepository;

        public AdminService(GenericRepository<Admin> adminRepository,
                            GenericRepository<Advertisement> advertisementsRepository,
                            GenericRepository<Class> classRepository,
                            GenericRepository<DayBook> dayBookRepository,
                            GenericRepository<DayOfWeek> dayOfWeekRepository,
                            GenericRepository<LessonTime> lessonTimeRepository,
                            GenericRepository<Parents> parentsRepository,
                            GenericRepository<ParentsPupil> parentsPupilRepository,
                            GenericRepository<Pupil> pupilRepository,
                            GenericRepository<Schedule> scheduleRepository,
                            GenericRepository<Subject> subjectRepository,
                            GenericRepository<Teacher> teacherRepository,
                            GenericRepository<User> userRepository)
        {
            this.advertisementsRepository = advertisementsRepository;
            this.classRepository = classRepository;
            this.dayBookRepository = dayBookRepository;
            this.dayOfWeekRepository = dayOfWeekRepository;
            this.parentsRepository = parentsRepository;
            this.parentsPupilRepository = parentsPupilRepository;
            this.pupilRepository = pupilRepository;
            this.subjectRepository = subjectRepository;
            this.teacherRepository = teacherRepository;
            this.userRepository = userRepository;
        }

        public List<Class> GetAllClasses()
        {
            var classes = this.classRepository.GetAll().OrderBy(x => x.ClassName).ToList();
            return classes;
        }

        public List<User> GetAllAdmins()
        {
            var admines = this.userRepository.GetAll().Where(x => x.Role == "адмін").OrderByDescending(x => x.LastName).ToList();
            return admines;
        }

        public List<User> GetAllPupils()
        {
            var pupils = this.userRepository.GetAllq().Include(x => x.Pupil).Where(x => x.Role == "учень").OrderByDescending(x => x.LastName).ToList();
            return pupils;
        }

        public List<User> GetAllTeacher()
        {
            var teacher = this.userRepository.GetAll().Where(x => x.Role == "вчитель").OrderByDescending(x => x.LastName).ToList();
            return teacher;
        }

        public List<User> GetAllParents()
        {
            var parents = this.userRepository.GetAll().Where(x => x.Role == "батьки").OrderByDescending(x => x.LastName).ToList();
            return parents;
        }

        public List<User> GetAllPupilsForClass(int classId)
        {
            var userPupils = GetAllPupils().Where(x => x.Pupil.ClassId == classId).OrderByDescending(x => x.LastName).ToList();
            return userPupils;
        }

        public List<Subject?> GetAllSubjectsForTeacher(int teacherId)
        {
            var subjectsWithTeachers = this.subjectRepository.GetAllq()
                .Include(x => x.Teachers)
                .Where(x => x.Teachers!.Select(y => y.UserId).Contains(teacherId)).OrderByDescending(x => x.SubjectName).ToList();
            return subjectsWithTeachers!;
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

        public int GetNewDayBookId()
        {
            return (this.dayBookRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewDayOfWeekId()
        {
            return (this.dayOfWeekRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewUserId()
        {
            return (this.userRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewPupilId()
        {
            return (this.pupilRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewTeacherId()
        {
            return (this.teacherRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewSubjectId()
        {
            return (this.subjectRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewParentId()
        {
            return (this.parentsRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewClassId()
        {
            return (this.classRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        public int GetNewAdvertisementId()
        {
            return (this.advertisementsRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        }

        //public int GetNewParentPupilId()
        //{
        //    return (this.parentsPupilRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
        //}
    }
}
