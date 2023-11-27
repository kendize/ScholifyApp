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
    public class JournalService
    {
        public IGenericRepository<DayOfWeek> dayOfWeekRepository;
        public IGenericRepository<DayBook> dayBookRepository;
        public IGenericRepository<Subject> subjectRepository;
        public IGenericRepository<Class> classRepository;
        public ScheduleService scheduleService;
        public JournalService(GenericRepository<DayBook> dayBookRepository, GenericRepository<Subject> subjectRepository, GenericRepository<Class> classRepository, GenericRepository<DayOfWeek> dayOfWeekRepository, ScheduleService scheduleService) 
        {
            this.scheduleService = scheduleService;
            this.dayBookRepository = dayBookRepository;
            this.subjectRepository = subjectRepository;
            this.classRepository = classRepository;
            this.dayOfWeekRepository = dayOfWeekRepository;
        }

        public List<DayBook> GetDayBooks(int classId)
        {
            var result = this.dayBookRepository.GetAllq()
                .Include(x => x.Pupil)
                .ThenInclude(x => x.User)
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Subject)
                .Include(x => x.Schedule)
                .ThenInclude(x => x.DayOfWeek)
                //.Where(x => x.Pupil.ClassId == classId)
                .AsNoTracking().ToList();

            return result;
        }

        public List<DayBook> GetDayBooksForUserId(int userId)
        {
            var result = this.dayBookRepository.GetAllq()
                .Include(x => x.Pupil)
                .ThenInclude(x => x.User)
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Subject)
                .Include(x => x.Schedule)
                .ThenInclude(x => x.DayOfWeek)
                .Where(x => x.Pupil.User.Id == userId)
                .AsNoTracking().ToList();

            return result;
        }

        public void AddGrade(DayOfWeek dayOfWeek, DayBook dayBook, int subjectId)
        {
            if (this.dayOfWeekRepository.GetAll().FirstOrDefault(x => x.Date.Date.Equals(dayOfWeek!.Date.Date)) != null)
            {
                dayOfWeek = this.dayOfWeekRepository.GetAll().FirstOrDefault(x => x.Date.Date.Equals(dayOfWeek!.Date.Date))!;
                var dayOfWeekId = dayOfWeek != null ? dayOfWeek!.Id : 0;
                var schedulesForDay = new List<Schedule>();
            }
            else
            {
                this.dayOfWeekRepository.Insert(dayOfWeek);
                this.dayOfWeekRepository.Save();
            }

            var subject = this.subjectRepository.GetAll().FirstOrDefault(x => x.Id == subjectId);

            var schedule = this.scheduleService.GetAllSchedulesForSubjectId(subjectId).FirstOrDefault(x => x.DayOfWeek.Day.Equals(dayOfWeek.Day));

            dayBook.ScheduleId = schedule.Id;

            this.dayBookRepository.Insert(dayBook);
            this.dayBookRepository.Save();
        }

        public void DeleteGrade(int gradeId)
        {
            this.dayBookRepository.Delete(gradeId);
            this.dayBookRepository.Save();
        }
        //public List<DayBook> GetDayBooksBySubjectId(int subjectId)
        //{
        //    var result = this.dayBookRepository.GetAllq()
        //        .Include(x => x.Pupil)
        //        .Include(x => x.Schedule)
        //        .Where(x => x.Schedule!.Id == subjectId).ToList();

        //    return result;
        //}

    }
}
