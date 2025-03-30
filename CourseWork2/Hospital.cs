using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CourseWork2
{
    internal class Hospital
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("obl")]
        public string Obl { get; set; }

        [Column("hosp")]
        public string Hosp { get; set; }

        [Column("doctor")]
        public string Doctor { get; set; }

        [Column("time")]
        public string Time { get; set; }

        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string MiddleName { get; set; }
        //public string SNILS { get; set; }
        //public string Address { get; set; }

        public Hospital() { }

        public Hospital(string obl, string hosp, string doctor, string time)
        {
            this.Obl = obl;
            this.Hosp = hosp;
            this.Doctor = doctor;
            this.Time = time;
        }
    }
}
