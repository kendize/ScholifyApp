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
    public class DayBookService
    {
        public GenericRepository<DayBook> dayBookRepository;
        public DayBookService(GenericRepository<DayBook> dayOfWeekRepository)
        {
            this.dayBookRepository = dayOfWeekRepository;
        }

        public List<DayBook> GetAll()
        {
            return this.dayBookRepository.GetAll().ToList();
        }
    }
}
