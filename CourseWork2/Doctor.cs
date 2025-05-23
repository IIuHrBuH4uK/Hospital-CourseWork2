﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CourseWork2
{
    internal class Doctor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("spec_id")]
        public int SpecId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("age")]
        public string Age { get; set; }

        [Column("exp")]
        public string Exp { get; set; }

        [Column("time")]
        public string Time { get; set; }


        public Doctor() { }

        public Doctor(int spec_id, string name, string age, string exp, string time)
        {
            Id = spec_id;
            Name = name;
            Age = age;
            Exp = exp;
            Time = time;
        }
    }
}
