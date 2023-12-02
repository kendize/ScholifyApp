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
    public class LessonTimeService
    {
        public GenericRepository<LessonTime> lessonTimeRepository;
        public LessonTimeService(GenericRepository<LessonTime> lessonTimeRepository)
        {
            this.lessonTimeRepository = lessonTimeRepository;
        }

        public List<LessonTime> GetAllLessonTimes()
        {
            return this.lessonTimeRepository.GetAll().ToList();
        }

        public LessonTime GetLessonTimeById(int id)
        {
            return this.lessonTimeRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
        }

    }
}
