using AnnuaireEntreprise.Data;
using AnnuaireEntreprise.Models;
using AnnuaireEntreprise.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace AnnuaireEntreprise.Views
{
    public partial class AdminWindow : Window
    {
        private AppDbContext _db = new AppDbContext();

        public AdminWindow()
        {
            InitializeComponent();
            ChargerTout();
        }

        private void ChargerTout()
        {
            DgAdminSalaries.ItemsSource = _db.Salaries
                .Include(s => s.Site)
                .Include(s => s.Service)
                .ToList();

            DgAdminSites.ItemsSource = _db.Sites.ToList();
            DgAdminServices.ItemsSource = _db.Services.ToList();
        }

        // ===== SALARIÉS =====
        private void BtnAjouterSalarie_Click(object sender, RoutedEventArgs e)
        {
            var form = new SalarieFormWindow(null);
            if (form.ShowDialog() == true)
            {
                LogService.LogAdminAccess($"Ajout salarié : {form.Salarie.Prenom} {form.Salarie.Nom}");
                _db.Salaries.Add(form.Salarie);
                _db.SaveChanges();
                ChargerTout();
            }
        }

        private void BtnModifierSalarie_Click(object sender, RoutedEventArgs e)
        {
            if (DgAdminSalaries.SelectedItem is not Salarie sel)
            {
                MessageBox.Show("Sélectionnez un salarié.", "Info");
                return;
            }
            var form = new SalarieFormWindow(sel);
            if (form.ShowDialog() == true)
            {
                LogService.LogAdminAccess($"Modification salarié ID={sel.Id}");
                var s = _db.Salaries.Find(sel.Id)!;
                s.Nom = form.Salarie.Nom;
                s.Prenom = form.Salarie.Prenom;
                s.Email = form.Salarie.Email;
                s.TelephoneFixe = form.Salarie.TelephoneFixe;
                s.TelephonePortable = form.Salarie.TelephonePortable;
                s.SiteId = form.Salarie.SiteId;
                s.ServiceId = form.Salarie.ServiceId;
                _db.SaveChanges();
                ChargerTout();
            }
        }

        private void BtnSupprimerSalarie_Click(object sender, RoutedEventArgs e)
        {
            if (DgAdminSalaries.SelectedItem is not Salarie sel)
            {
                MessageBox.Show("Sélectionnez un salarié.", "Info");
                return;
            }
            if (MessageBox.Show($"Supprimer {sel.Prenom} {sel.Nom} ?", "Confirmation",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LogService.LogAdminAccess($"Suppression salarié ID={sel.Id}");
                var s = _db.Salaries.Find(sel.Id)!;
                _db.Salaries.Remove(s);
                _db.SaveChanges();
                ChargerTout();
            }
        }

        // ===== SITES =====
        private void BtnAjouterSite_Click(object sender, RoutedEventArgs e)
        {
            var form = new SimpleFormWindow("Ajouter un site", "Ville :", "");
            if (form.ShowDialog() == true)
            {
                LogService.LogAdminAccess($"Ajout site : {form.Valeur}");
                _db.Sites.Add(new Site { Ville = form.Valeur });
                _db.SaveChanges();
                ChargerTout();
            }
        }

        private void BtnModifierSite_Click(object sender, RoutedEventArgs e)
        {
            if (DgAdminSites.SelectedItem is not Site sel)
            {
                MessageBox.Show("Sélectionnez un site.", "Info");
                return;
            }
            var form = new SimpleFormWindow("Modifier le site", "Ville :", sel.Ville);
            if (form.ShowDialog() == true)
            {
                LogService.LogAdminAccess($"Modification site ID={sel.Id}");
                var site = _db.Sites.Find(sel.Id)!;
                site.Ville = form.Valeur;
                _db.SaveChanges();
                ChargerTout();
            }
        }

        private void BtnSupprimerSite_Click(object sender, RoutedEventArgs e)
        {
            if (DgAdminSites.SelectedItem is not Site sel)
            {
                MessageBox.Show("Sélectionnez un site.", "Info");
                return;
            }
            if (MessageBox.Show($"Supprimer le site {sel.Ville} ?", "Confirmation",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LogService.LogAdminAccess($"Suppression site ID={sel.Id}");
                var site = _db.Sites.Find(sel.Id)!;
                _db.Sites.Remove(site);
                _db.SaveChanges();
                ChargerTout();
            }
        }

        // ===== SERVICES =====
        private void BtnAjouterService_Click(object sender, RoutedEventArgs e)
        {
            var form = new SimpleFormWindow("Ajouter un service", "Nom :", "");
            if (form.ShowDialog() == true)
            {
                LogService.LogAdminAccess($"Ajout service : {form.Valeur}");
                _db.Services.Add(new Service { Nom = form.Valeur });
                _db.SaveChanges();
                ChargerTout();
            }
        }

        private void BtnModifierService_Click(object sender, RoutedEventArgs e)
        {
            if (DgAdminServices.SelectedItem is not Service sel)
            {
                MessageBox.Show("Sélectionnez un service.", "Info");
                return;
            }
            var form = new SimpleFormWindow("Modifier le service", "Nom :", sel.Nom);
            if (form.ShowDialog() == true)
            {
                LogService.LogAdminAccess($"Modification service ID={sel.Id}");
                var service = _db.Services.Find(sel.Id)!;
                service.Nom = form.Valeur;
                _db.SaveChanges();
                ChargerTout();
            }
        }

        private void BtnSupprimerService_Click(object sender, RoutedEventArgs e)
        {
            if (DgAdminServices.SelectedItem is not Service sel)
            {
                MessageBox.Show("Sélectionnez un service.", "Info");
                return;
            }
            if (MessageBox.Show($"Supprimer le service {sel.Nom} ?", "Confirmation",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LogService.LogAdminAccess($"Suppression service ID={sel.Id}");
                var service = _db.Services.Find(sel.Id)!;
                _db.Services.Remove(service);
                _db.SaveChanges();
                ChargerTout();
            }
        }
    }
}