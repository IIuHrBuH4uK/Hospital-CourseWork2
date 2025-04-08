using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace CourseWork2
{
    internal class Call
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Symptoms { get; set; }

        public Call() { }

        public Call(int userid, string firstname, string lastname, string middlename, string address, string phone, string symptoms)
        {
            UserId = userid;
            FirstName = firstname;
            LastName = lastname;
            MiddleName = middlename;
            Address = address;
            Phone = phone;
            Symptoms = symptoms;
        }
    }
}
