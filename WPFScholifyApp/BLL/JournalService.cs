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
    public class JournalService
    {
        public IGenericRepository<DayBook> dayBookRepository;
        public IGenericRepository<Subject> subjectRepository;
        public IGenericRepository<Class> classRepository;
        public JournalService(GenericRepository<DayBook> dayBookRepository, GenericRepository<Subject> subjectRepository, GenericRepository<Class> classRepository) 
        {
            this.dayBookRepository = dayBookRepository;
            this.subjectRepository = subjectRepository;
            this.classRepository = classRepository;
        }

        public List<DayBook> GetDayBooks(int userId) 
        {
            var result = this.dayBookRepository.GetAllq()
                .Include(x => x.Pupil)
                .ThenInclude(x => x.User)
                .Include(x => x.Schedule)
                .ThenInclude(x => x.DayOfWeek).ToList();

            return result;
        }

        public List<DayBook> GetDayBooks(int userId, int classId)
        {
            var result = this.dayBookRepository.GetAllq()
                .Include(x => x.Pupil)
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Subject)
                .Where(x => x.Pupil.ClassId == classId).ToList();

            return result;
        }

        public void AddGrade()
        {
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
