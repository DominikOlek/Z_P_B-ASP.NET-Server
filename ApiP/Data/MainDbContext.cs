using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Data
{
    public class MainDbContext : DbContext
    {
        private string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ApiP;Trusted_Connection=True;";

        public DbSet<Users> UsersBd { get; set; }
        public DbSet<Role> RolesBd { get; set; }
        public DbSet<Kierowcy> KierowcyBd { get; set; }
        public DbSet<Mandaty> MandatyBd { get; set; }
        public DbSet<Taryfikator> TaryfikatorBd { get; set; }
        public DbSet<Historia> HistBd { get; set; }
        public DbSet<CzasowoOdebrane> CzasowoBd { get; set; }
        public DbSet<AppUsers> AppUsersBd { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().Property(u => u.Nr_Sluzbowy).IsRequired();
            modelBuilder.Entity<Users>().Property(u => u.Imie).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Users>().Property(u => u.Nazwisko).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Users>().Property(u => u.Pesel).IsRequired().HasMaxLength(11);
            modelBuilder.Entity<Users>().Property(u => u.PasswordHash).IsRequired();
            modelBuilder.Entity<Role>().Property(u => u.Nazwa_Roli).IsRequired();

            modelBuilder.Entity<Kierowcy>().Property(u => u.Pesel).IsRequired().HasMaxLength(11);
            modelBuilder.Entity<Kierowcy>().Property(u => u.Imie).IsRequired();
            modelBuilder.Entity<Kierowcy>().Property(u => u.Nazwisko).IsRequired();
            modelBuilder.Entity<Mandaty>().Property(u => u.PowodID).IsRequired();
            modelBuilder.Entity<Mandaty>().Property(u => u.DataWydania).IsRequired();
            modelBuilder.Entity<Taryfikator>().Property(u => u.Liczba_PKT).IsRequired();
            modelBuilder.Entity<CzasowoOdebrane>().Property(u => u.KierowcaID).IsRequired();

            modelBuilder.Entity<AppUsers>().Property(u => u.Imie).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<AppUsers>().Property(u => u.Nazwisko).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<AppUsers>().Property(u => u.Pesel).IsRequired().HasMaxLength(11);
            modelBuilder.Entity<AppUsers>().Property(u => u.PasswordHash).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(_connectionString);
        }
    }
}
