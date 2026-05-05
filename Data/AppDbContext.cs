using AnnuaireEntreprise.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnuaireEntreprise.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Salarie> Salaries { get; set; } // DbSet = récupérer les données de la table Salarie
        public DbSet<Site> Sites { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) // initialiser la connexion à la base de données
        {
            options.UseSqlite("Data Source=annuaire.db"); // utiliser SQLite et créer un fichier annuaire.db 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // configurer les relations entre les entités
        {
            modelBuilder.Entity<Salarie>() // modelBuilder permet de configurer les relations entre les entités
                .HasOne(s => s.Site) 
                .WithMany(si => si.Salaries) 
                .HasForeignKey(s => s.SiteId); 

            modelBuilder.Entity<Salarie>()
                .HasOne(s => s.Service) //HasOne = un salarié a un site
                .WithMany(sv => sv.Salaries) // WithMany = un site peut avoir plusieurs salariés
                .HasForeignKey(s => s.ServiceId); // la clé étrangère est ServiceId dans la table Salarie
        }
    }
}