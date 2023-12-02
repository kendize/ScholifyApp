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
    public class DayOfWeekService
    {
        public GenericRepository<DayOfWeek> dayOfWeekRepository;
        public DayOfWeekService(GenericRepository<DayOfWeek> dayOfWeekRepository)
        {
            this.dayOfWeekRepository = dayOfWeekRepository;
        }

        public List<DayOfWeek> GetAll()
        {
            return this.dayOfWeekRepository.GetAll().ToList();
        }

        public void Save(DayOfWeek dayOfWeek)
        {
            this.dayOfWeekRepository.Insert(dayOfWeek);
            this.dayOfWeekRepository.Save();
        }
    }
}
