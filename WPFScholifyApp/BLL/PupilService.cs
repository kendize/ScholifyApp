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


        private GenericRepository<Pupil> pupilRepository;
        private GenericRepository<Schedule> scheduleRepository;

        public PupilService( GenericRepository<Pupil> pupilRepository, GenericRepository<Schedule> scheduleRepository)
        {
            this.pupilRepository = pupilRepository;
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

        public List<Pupil> GetAllPupils()
        {
            return this.pupilRepository.GetAllq().Include(x => x.ParentsPupil)!.ThenInclude(x => x.parent).ToList().ToList();
        }
    }
}
