using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScholifyApp.DAL.DBClasses
{
    public class ParentsPupil
    {
        //[Key]
        //public int Id { get; set; }
        public int parentId { get;set; }
        public Parents? parent { get; set; }
        public int pupilId { get;set; }
        public Pupil? pupil { get; set; }
    }
}
