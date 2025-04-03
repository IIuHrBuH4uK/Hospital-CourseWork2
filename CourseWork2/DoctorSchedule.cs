using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork2
{
    internal class DoctorSchedule
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [Column("date")]
        public string Date { get; set; }

        [Column("time")]
        public string Time { get; set; }


        [Column("is_brooked")]
        public bool IsBrooked { get; set; }

        public DoctorSchedule() { }

        public DoctorSchedule(int doctorid, string date, string time, bool isbroked)
        {
            Id = doctorid;
            Date = date;
            Time = time;
            IsBrooked = isbroked;
        }
    }
}
