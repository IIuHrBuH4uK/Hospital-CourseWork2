using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;


namespace CourseWork2
{
    internal class User
    {
        [Key] 
        [Column("id")]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SNILS { get; set; }
        public string Address { get; set; }

        public string Gender   { get; set; }
        public string Birthday { get; set; }

        public User() { }

        public User(string login, string email, string password)
        {
            this.Login = login;
            this.Email = email;
            this.Password = password;
        }
    }
}
