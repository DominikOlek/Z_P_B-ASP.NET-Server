using System.Threading.Tasks;
using ApiP.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ApiP.Data
{
    public class AppUsers
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Nr_tel { get; set; }
        public DateTime BirthDate { get; set; }
        public string Pesel { get; set; }
        public string Nr_telHelp { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
