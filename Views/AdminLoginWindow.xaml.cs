using AnnuaireEntreprise.Services;
using System.Windows;

namespace AnnuaireEntreprise.Views
{
    public partial class AdminLoginWindow : Window // HERITAGE de la classe Window
    { //SLIDE 6 (B) - Fenêtre de connexion admin
        private const string MotDePasseAdmin = "admin123"; // mdp choisi pour l'accès admin

        public AdminLoginWindow()
        {
            InitializeComponent();
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            if (TxtPassword.Password == MotDePasseAdmin)
            {
                LogService.LogAdminAccess("Connexion réussie");
                this.Close();
                var adminWindow = new AdminWindow();
                adminWindow.ShowDialog();
            }
            else
            {
                LogService.LogAdminAccess("Tentative de connexion échouée");
                TxtErreur.Text = "Mot de passe incorrect !";
                TxtErreur.Visibility = Visibility.Visible;
            }
        }
    }
}