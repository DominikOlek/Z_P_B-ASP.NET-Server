using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using ApiP.Data;
using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ApiP
{
    public class DbMigration
    {
        private readonly MainDbContext dbContext;
        private readonly IPasswordHasher<Users> _hasher;

        public DbMigration(MainDbContext _DbContext, IPasswordHasher<Users> Hasher)
        {
            dbContext = _DbContext;
            _hasher = Hasher;
        }
        public void Seed() {
            if (dbContext.Database.CanConnect())
            {
                var pendingMigration = dbContext.Database.GetPendingMigrations();
                if (pendingMigration != null || !pendingMigration.Any())
                {
                    dbContext.Database.Migrate();
                }
                if (!dbContext.TaryfikatorBd.Any()) {
                    var taryfikaty = GetTar();
                    dbContext.TaryfikatorBd.AddRange(taryfikaty);
                }
                if (!dbContext.RolesBd.Any())
                {
                    var roles = GetRoles();
                    dbContext.RolesBd.AddRange(roles);
                }
                dbContext.SaveChanges();
               /* if (!dbContext.UsersBd.Any())
                {
                    var users = GetAdmin();
                    dbContext.UsersBd.AddRange(users);
                }
                dbContext.SaveChanges();*/
            }
        }
        public IEnumerable<Taryfikator> GetTar()
        {
            string TaryfFile = File.ReadAllText(@"D:\Nowy folder\ApiP\JSON\Taryfikaty.json");
            IEnumerable<Taryfikator> rol = null;
            try
            {
                rol = JsonConvert.DeserializeObject<List<Taryfikator>>(TaryfFile);
            }
            catch { 
                
            }
            foreach (Taryfikator i in rol) {
                i.ID = 0;
            }
            return rol;
        }

        public IEnumerable<Users> GetAdmin()
        {
            IEnumerable<Users> use = new Users[] {
                new Users(){
                    RolaID=7,
                    Imie="Admin",
                    Nazwisko="Admin",
                    Nr_Sluzbowy=10,
                    Email="adminstrony@gmail.com",
                    Pesel="12345678109",
                    PasswordHash="",
                    Aktywny = true
                }
            };
            var Password = _hasher.HashPassword(use.First(), "zaq1qwerty");
            use.First().PasswordHash = Password;
            return use;
           
        }

        public IEnumerable<Role> GetRoles() {
            IEnumerable<Role> rol= new Role[] { 
                new Role(){ 
                    Nazwa_Roli="Ruch_Drogowy",
                },
                new Role(){
                    Nazwa_Roli="Komendant",
                },
                new Role(){
                    Nazwa_Roli="Administrator",
                }
            };
            return rol;
        }
    }
}

