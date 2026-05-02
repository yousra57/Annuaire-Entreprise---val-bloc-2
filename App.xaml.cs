using AnnuaireEntreprise.Data;
using AnnuaireEntreprise.Services;
using System.Windows;

namespace AnnuaireEntreprise
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseInitializer.Initialize();

            var db = new AppDbContext();
            if (!db.Salaries.Any())
            {
                var api = new ApiService();
                await api.ImportUsersAsync();
            }
        }
    }
}