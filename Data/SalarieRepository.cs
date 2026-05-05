using AnnuaireEntreprise.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace AnnuaireEntreprise.Data
{
    /// <summary>
    /// PATTERN REPOSITORY combinant requêtes SQL brutes et ORM (Entity Framework) pour répondre à la grille BLOC 2
    /// </summary>
    public class SalarieRepository
    {
        private const string ConnectionString = "Data Source=annuaire.db";

        // REQUÊTES SQL BRUTES

        /// <summary>
        /// Recherche des salariés par nom (saisie partielle) — SQL brut, purement écrit à la main 
        /// </summary>
        public List<Salarie> RechercherParNomSQL(string nom)
        {
            var salaries = new List<Salarie>();

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT s.Id, s.Nom, s.Prenom, s.Email, 
                       s.TelephoneFixe, s.TelephonePortable,
                       s.SiteId, s.ServiceId,
                       si.Ville AS SiteVille,
                       sv.Nom AS ServiceNom
                FROM Salaries s
                INNER JOIN Sites si ON s.SiteId = si.Id
                INNER JOIN Services sv ON s.ServiceId = sv.Id
                WHERE s.Nom LIKE @nom OR s.Prenom LIKE @nom
                ORDER BY s.Nom, s.Prenom";

            command.Parameters.AddWithValue("@nom", $"%{nom}%");

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                salaries.Add(new Salarie
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Prenom = reader.GetString(2),
                    Email = reader.GetString(3),
                    TelephoneFixe = reader.GetString(4),
                    TelephonePortable = reader.GetString(5),
                    SiteId = reader.GetInt32(6),
                    ServiceId = reader.GetInt32(7),
                    Site = new Site { Ville = reader.GetString(8) },
                    Service = new Service { Nom = reader.GetString(9) }
                });
            }

            return salaries;
        }

        /// <summary>
        /// Récupère tous les salariés d'un site — SQL brut
        /// </summary>
        public List<Salarie> RechercherParSiteSQL(int siteId)
        {
            var salaries = new List<Salarie>();

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT s.Id, s.Nom, s.Prenom, s.Email,
                       s.TelephoneFixe, s.TelephonePortable,
                       s.SiteId, s.ServiceId,
                       si.Ville AS SiteVille,
                       sv.Nom AS ServiceNom
                FROM Salaries s
                INNER JOIN Sites si ON s.SiteId = si.Id
                INNER JOIN Services sv ON s.ServiceId = sv.Id
                WHERE s.SiteId = @siteId
                ORDER BY s.Nom";

            command.Parameters.AddWithValue("@siteId", siteId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                salaries.Add(new Salarie
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Prenom = reader.GetString(2),
                    Email = reader.GetString(3),
                    TelephoneFixe = reader.GetString(4),
                    TelephonePortable = reader.GetString(5),
                    SiteId = reader.GetInt32(6),
                    ServiceId = reader.GetInt32(7),
                    Site = new Site { Ville = reader.GetString(8) },
                    Service = new Service { Nom = reader.GetString(9) }
                });
            }

            return salaries;
        }

        /// <summary>
        /// Compte le nombre de salariés par service — SQL brut
        /// </summary>
        public Dictionary<string, int> CompterSalariesParService()
        {
            var resultats = new Dictionary<string, int>();

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT sv.Nom, COUNT(s.Id) AS NbSalaries
                FROM Services sv
                LEFT JOIN Salaries s ON s.ServiceId = sv.Id
                GROUP BY sv.Id, sv.Nom
                ORDER BY NbSalaries DESC";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                resultats[reader.GetString(0)] = reader.GetInt32(1);
            }

            return resultats;
        }


        // REQUÊTES ORM (Entity Framework Core)

        /// <summary>
        /// Recherche des salariés par nom — ORM
        /// </summary>
        public List<Salarie> RechercherParNomORM(string nom) // LINQ
        {
            using var db = new AppDbContext();
            return db.Salaries
                .Include(s => s.Site) // charge les données du site lié
                .Include(s => s.Service) // charge les données du service lié
                .Where(s => s.Nom.Contains(nom) || s.Prenom.Contains(nom)) // filtre par nom
                .OrderBy(s => s.Nom)  // trie par ordre alphabétique
                .ToList(); // exécute et retourne une liste
        }

        /// <summary>
        /// Récupère tous les salariés d'un service — ORM
        /// </summary>
        public List<Salarie> RechercherParServiceORM(int serviceId)
        {
            using var db = new AppDbContext();
            return db.Salaries
                .Include(s => s.Site)
                .Include(s => s.Service)
                .Where(s => s.ServiceId == serviceId)
                .OrderBy(s => s.Nom)
                .ToList();
        }

        /// <summary>
        /// Ajoute un salarié — ORM
        /// </summary>
        public void AjouterORM(Salarie salarie)
        {
            using var db = new AppDbContext();
            db.Salaries.Add(salarie);
            db.SaveChanges();
        }

        /// <summary>
        /// Supprime un salarié par ID — ORM
        /// </summary>
        public void SupprimerORM(int id)
        {
            using var db = new AppDbContext();
            var salarie = db.Salaries.Find(id);
            if (salarie != null)
            {
                db.Salaries.Remove(salarie);
                db.SaveChanges();
            }
        }
    }
}