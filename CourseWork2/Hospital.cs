using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork2
{
    internal class Hospital
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("region_id")]
        public int RegionId { get; set; }

        [Column("hospital")]
        public string Hosp { get; set; }

        [Column("address")]
        public string Address {  get; set; }

        public Hospital() { }

        public Hospital(int regionid, string hospital, string address)
        {
            this.RegionId = regionid;
            this.Hosp = hospital;
            this.Address = address;
        }
    }
}
