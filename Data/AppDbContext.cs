using AnnuaireEntreprise.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnuaireEntreprise.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Salarie> Salaries { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=annuaire.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salarie>()
                .HasOne(s => s.Site)
                .WithMany(si => si.Salaries)
                .HasForeignKey(s => s.SiteId);

            modelBuilder.Entity<Salarie>()
                .HasOne(s => s.Service)
                .WithMany(sv => sv.Salaries)
                .HasForeignKey(s => s.ServiceId);
        }
    }
}