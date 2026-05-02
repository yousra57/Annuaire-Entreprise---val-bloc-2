using AnnuaireEntreprise.Data;
using System.Windows;

namespace AnnuaireEntreprise
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseInitializer.Initialize();
        }
    }
}