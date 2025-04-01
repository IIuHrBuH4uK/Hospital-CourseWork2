using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork2
{
    internal class Specialization
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("hospital_id")]
        public int HospitalId { get; set; }

        [Column("spec")]
        public string Spec { get; set; }

        public Specialization() { }

        public Specialization(int hospitalid, string spec)
        {
            this.HospitalId = hospitalid;
            this.Spec = spec;
        }
    }
}
