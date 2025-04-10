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

        [Column("region")]
        public string Region { get; set; }

        [Column("hospital")]
        public string Hospital { get; set; }

        [Column("specialization")]
        public string Specialization { get; set; }

        [Column("doctor")]
        public string Doctor { get; set; }

        [Column("date")]
        public string Date { get; set; }

        [Column("numbertalon")]
        public string Numbertalon { get; set; }

        public string RegionDisplay => $"Область: {Region}";
        public string HospitalDisplay => $"Больница: {Hospital}";
        public string SpecializationDisplay => $"Специальность: {Specialization}";
        public string DoctorDisplay => $"Врач: {Doctor}";
        public string DateDisplay => $"Дата: {Date}";
        public string NumbertalonDisplay => $"Номер талона: {Numbertalon}";

        public Ticket() { }

        public Ticket(int userid, string region, string hospital, string specialization, string doctor, string date, string numbertalon)
        {
            UserId = userid;
            Region = region;
            Hospital = hospital;
            Specialization = specialization;
            Doctor = doctor;
            Date = date;
            Numbertalon = numbertalon;
        }
    }
}
