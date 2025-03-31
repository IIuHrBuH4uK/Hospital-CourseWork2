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
        internal class Region
        {
            [Key]
            [Column("id")]
            public int Id { get; set; }

            [Column("region")]
            public string Reg { get; set; }

            public Region() { }

            public Region(int id,string region)
            {
                this.Id = id;
                this.Reg =region;
            }
        }
    }
