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
        public DbSet<Roles> RolesBd { get; set; }
        public DbSet<Drivers> DriversBd { get; set; }
        public DbSet<Tickets> TicketsBd { get; set; }
        public DbSet<Taryfikator> TaryfikatBd { get; set; }
        public DbSet<History> HistBd { get; set; }
        public DbSet<TimelyLost> TimelyBd { get; set; }
        public DbSet<AppUsers> AppUsersBd { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().Property(u => u.BadgeNumber).IsRequired();
            modelBuilder.Entity<Users>().Property(u => u.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Users>().Property(u => u.LastName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Users>().Property(u => u.Pesel).IsRequired().HasMaxLength(11);
            modelBuilder.Entity<Users>().Property(u => u.PasswordHash).IsRequired();
            modelBuilder.Entity<Roles>().Property(u => u.RoleName).IsRequired();

            modelBuilder.Entity<Drivers>().Property(u => u.Pesel).IsRequired().HasMaxLength(11);
            modelBuilder.Entity<Drivers>().Property(u => u.Name).IsRequired();
            modelBuilder.Entity<Drivers>().Property(u => u.LastName).IsRequired();
            modelBuilder.Entity<Tickets>().Property(u => u.ReasonID).IsRequired();
            modelBuilder.Entity<Tickets>().Property(u => u.DateOfTicket).IsRequired();
            modelBuilder.Entity<Taryfikator>().Property(u => u.PointNumber).IsRequired();
            modelBuilder.Entity<TimelyLost>().Property(u => u.DriverID).IsRequired();

            modelBuilder.Entity<AppUsers>().Property(u => u.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<AppUsers>().Property(u => u.LastName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<AppUsers>().Property(u => u.Pesel).IsRequired().HasMaxLength(11);
            modelBuilder.Entity<AppUsers>().Property(u => u.PasswordHash).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(_connectionString);
        }
    }
}
