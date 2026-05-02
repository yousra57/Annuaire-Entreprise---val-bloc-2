using AnnuaireEntreprise.Services;
using System.Windows;

namespace AnnuaireEntreprise.Views
{
    public partial class AdminLoginWindow : Window
    {
        private const string MotDePasseAdmin = "admin123";

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