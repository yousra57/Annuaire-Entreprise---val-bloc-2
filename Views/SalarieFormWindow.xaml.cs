using AnnuaireEntreprise.Data;
using AnnuaireEntreprise.Models;
using System.Windows;

namespace AnnuaireEntreprise.Views
{
    public partial class SalarieFormWindow : Window // classe pour ajouter/modifier un salarié, utilisée dans AdminWindow
    {
        public Salarie Salarie { get; private set; } = new();
        private AppDbContext _db = new AppDbContext();

        public SalarieFormWindow(Salarie? salarie)
        {
            InitializeComponent();

            CbSite.ItemsSource = _db.Sites.ToList();
            CbService.ItemsSource = _db.Services.ToList();

            if (salarie != null)
            {
                Title = "Modifier un salarié";
                TxtNom.Text = salarie.Nom;
                TxtPrenom.Text = salarie.Prenom;
                TxtEmail.Text = salarie.Email;
                TxtTelFixe.Text = salarie.TelephoneFixe;
                TxtTelPortable.Text = salarie.TelephonePortable;
                CbSite.SelectedValue = salarie.SiteId;
                CbService.SelectedValue = salarie.ServiceId;
            }
            else
            {
                Title = "Ajouter un salarié";
            }
        }
    // Valider la saisie et créer/modifier l'objet Salarie
        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNom.Text) ||
                string.IsNullOrWhiteSpace(TxtPrenom.Text) ||
                CbSite.SelectedItem == null ||
                CbService.SelectedItem == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur");
                return;
            }

            Salarie = new Salarie
            {
                Nom = TxtNom.Text.Trim().ToUpper(),
                Prenom = TxtPrenom.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                TelephoneFixe = TxtTelFixe.Text.Trim(),
                TelephonePortable = TxtTelPortable.Text.Trim(),
                SiteId = ((Site)CbSite.SelectedItem).Id,
                ServiceId = ((Service)CbService.SelectedItem).Id
            };
            DialogResult = true;
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}