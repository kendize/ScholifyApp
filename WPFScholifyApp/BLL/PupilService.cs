using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScholifyApp.DAL.ClassRepository;
using WPFScholifyApp.DAL.DBClasses;

namespace WPFScholifyApp.BLL
{
    public class PupilService
    {
        private IGenericRepository<User> userRepository;

        private IGenericRepository<Class> classRepository;

        private IGenericRepository<Teacher> teacherRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<Admin> adminRepository;
        private IGenericRepository<Parents> parentsRepository;
        private IGenericRepository<Subject> subjectRepository;
        private IGenericRepository<Schedule> scheduleRepository;

        public PupilService(IGenericRepository<User> userRepos, IGenericRepository<Class> classRepository, IGenericRepository<Teacher> teacherRepository, IGenericRepository<Pupil> pupilRepository, IGenericRepository<Admin> adminRepository, IGenericRepository<Parents> parentsRepository, IGenericRepository<Subject> subjectRepository, IGenericRepository<Schedule> scheduleRepository)
        {
            this.userRepository = userRepos;
            this.classRepository = classRepository;
            this.teacherRepository = teacherRepository;
            this.parentsRepository = parentsRepository;
            this.pupilRepository = pupilRepository;
            this.adminRepository = adminRepository;
            this.subjectRepository = subjectRepository;
            this.scheduleRepository = scheduleRepository;
        }

        public List<Schedule> GetAllSchedules(int classId,int dayOfWeek)
        {
            return this.scheduleRepository.GetAllq()
                .Include(x => x.DayOfWeek)
                .Include(x => x.Class)
                .Include(x => x.Teacher)
                .Include(x => x.LessonTime)
                .Include(x => x.Subject)
                .Where(x => x.Class!.Id == classId && x.DayOfWeekId == dayOfWeek).ToList();
        }

        public List<Pupil> GetAddPupils()
        {
            return this.pupilRepository.GetAll().ToList();
        }
    }
}
