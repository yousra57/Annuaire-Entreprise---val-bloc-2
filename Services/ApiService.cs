using System.Net.Http;
using AnnuaireEntreprise.Data;
using AnnuaireEntreprise.Models;
using Newtonsoft.Json.Linq;

namespace AnnuaireEntreprise.Services
{
    public class ApiService
    {
        private const string ApiUrl = "https://randomuser.me/api/?results=10&nat=fr"; // URL de l'API pour récupérer des utilisateurs aléatoires

        public async Task ImportUsersAsync()
        {
            using var http = new HttpClient();
    // SLIDE 3 - IMPORT DE DONNÉES DEPUIS UNE API
            var response = await http.GetStringAsync(ApiUrl); // envoyer une requête GET à l'API et récupérer la réponse sous forme de chaîne de caractères
            var json = JObject.Parse(response); // réponse JSON parsée en objet JObject pour pouvoir accéder aux données
            var results = json["results"]!.ToArray(); // tableau d'utilisateurs récupérés depuis l'API

            using var db = new AppDbContext();

            if (!db.Sites.Any()) // si la table Sites est vide, ajouter des sites prédéfinis
            {
                db.Sites.AddRange( // ajouter des sites prédéfinis
                    new Site { Ville = "Paris" },
                    new Site { Ville = "Lyon" },
                    new Site { Ville = "Marseille" },
                    new Site { Ville = "Bordeaux" },
                    new Site { Ville = "Lille" }
                );
                db.SaveChanges();
            }

            if (!db.Services.Any()) //et si la table Services est vide, ajouter des services prédéfinis
            {
                db.Services.AddRange( // ajouter des services prédéfinis
                    new Service { Nom = "Comptabilité" },
                    new Service { Nom = "Production" },
                    new Service { Nom = "Accueil" },
                    new Service { Nom = "Informatique" },
                    new Service { Nom = "RH" }
                );
                db.SaveChanges();
            }

            var sites = db.Sites.ToList(); 
            var services = db.Services.ToList();
            var rng = new Random();

            foreach (var user in results)// foreach pour parcourir les utilisateurs récupérés depuis l'API 
            {
                var salarie = new Salarie //
                {
                    Nom = user["name"]!["last"]!.ToString().ToUpper(),
                    Prenom = user["name"]!["first"]!.ToString(),
                    Email = user["email"]!.ToString(),
                    TelephoneFixe = user["phone"]!.ToString(),
                    TelephonePortable = user["cell"]!.ToString(),
                    SiteId = sites[rng.Next(sites.Count)].Id,
                    ServiceId = services[rng.Next(services.Count)].Id
                };
                db.Salaries.Add(salarie);
            }
            db.SaveChanges();
        }
    }
}