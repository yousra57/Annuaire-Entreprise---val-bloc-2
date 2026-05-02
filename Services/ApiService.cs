using System.Net.Http;
using AnnuaireEntreprise.Data;
using AnnuaireEntreprise.Models;
using Newtonsoft.Json.Linq;

namespace AnnuaireEntreprise.Services
{
    public class ApiService
    {
        private const string ApiUrl = "https://randomuser.me/api/?results=10&nat=fr";

        public async Task ImportUsersAsync()
        {
            using var http = new HttpClient();
            var response = await http.GetStringAsync(ApiUrl);
            var json = JObject.Parse(response);
            var results = json["results"]!.ToArray();

            using var db = new AppDbContext();

            if (!db.Sites.Any())
            {
                db.Sites.AddRange(
                    new Site { Ville = "Paris" },
                    new Site { Ville = "Lyon" },
                    new Site { Ville = "Marseille" },
                    new Site { Ville = "Bordeaux" },
                    new Site { Ville = "Lille" }
                );
                db.SaveChanges();
            }

            if (!db.Services.Any())
            {
                db.Services.AddRange(
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

            foreach (var user in results)
            {
                var salarie = new Salarie
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