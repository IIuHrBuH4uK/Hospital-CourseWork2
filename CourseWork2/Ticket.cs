using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork2
{
    internal class Ticket
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("region_id")]
        public int RegionId { get; set; }

        [Column("hospital_id")]
        public int HospitalId { get; set; }

        [Column("specialization_id")]
        public int SpecializationId { get; set; }

        [Column("schedule_id")]
        public int SheduleId { get; set; }

        [Column("created_at")]
        public string CreatedAT { get; set; }

        public Ticket() { }

        public Ticket(int id, int userid, int regionid, int hospitalid, int specializationid, int sheduleid, string createdat)
        {
            Id = id;
            UserId = userid;
            RegionId = regionid;
            HospitalId = hospitalid;
            SpecializationId = specializationid;
            SheduleId = sheduleid;
        }
    }
}
