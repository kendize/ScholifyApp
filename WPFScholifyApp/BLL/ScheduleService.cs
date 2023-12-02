using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScholifyApp.DAL.ClassRepository;
using WPFScholifyApp.DAL.DBClasses;
using DayOfWeek = WPFScholifyApp.DAL.DBClasses.DayOfWeek;

namespace WPFScholifyApp.BLL
{
    

    public class ScheduleService
    {
        private GenericRepository<Schedule> scheduleRepository;

        public ScheduleService( GenericRepository<Schedule> scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
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

        public List<DayOfWeek?> GetDatesBySubjectId(int subjectId)
        {
            return this.scheduleRepository.GetAllq().AsNoTracking().Include(x => x.DayOfWeek).Where(x => x.SubjectId == subjectId).Select(x => x.DayOfWeek).Distinct().ToList();
        }

        public List<DayOfWeek?> GetDatesByClassId(int classId)
        {
            return this.scheduleRepository.GetAllq().AsNoTracking().Include(x => x.Class).Where(x => x.ClassId == classId).Select(x => x.DayOfWeek).Distinct().ToList();
        }

        public void Save(Schedule schedule)
        {
            this.scheduleRepository.Insert(schedule);
            this.scheduleRepository.Save();
        }

        public void Delete(int schedule)
        {
            this.scheduleRepository.Delete(schedule);
            this.scheduleRepository.Save();
        }
    }
}
