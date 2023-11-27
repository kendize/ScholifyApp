namespace WPFScholifyApp.DAL.DBClasses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pupil
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int ClassId { get; set; }

        public virtual Class? Class { get; set; }

        public virtual ICollection<ParentsPupil>? ParentsPupil { get; set; }
    }
}
