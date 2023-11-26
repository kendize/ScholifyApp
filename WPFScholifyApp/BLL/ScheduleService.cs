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
    

    public class ScheduleService
    {
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Class> classRepository;
        private IGenericRepository<Schedule> scheduleRepository;
        private IGenericRepository<Subject> subjectRepository;

        public ScheduleService(IGenericRepository<User> userRepository, IGenericRepository<Class> classRepository, IGenericRepository<Schedule> scheduleRepository, IGenericRepository<Subject> subjectRepository)
        {
            this.userRepository = userRepository;
            this.classRepository = classRepository;
            this.scheduleRepository = scheduleRepository;
            this.subjectRepository = subjectRepository;
        }

        public List<Schedule> GetAllSchedulesForSubjectId(int subjectId)
        {
            return this.scheduleRepository.GetAllq()
                .Include(x => x.Subject)
                .Include(x => x.DayOfWeek)
                .Include(x => x.Class)
                .Include(x => x.LessonTime)
                .Where(x => x.Subject!.Id == subjectId).ToList();
        }



    }
}
