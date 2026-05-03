using AnnuaireEntreprise.Data;
using AnnuaireEntreprise.Models;
using AnnuaireEntreprise.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnnuaireEntreprise
{
    public partial class MainWindow : Window
    {
        private AppDbContext _db = new AppDbContext();
        private List<Salarie> _tousLesSalaries = new();

        public MainWindow()
        {
            InitializeComponent();
            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            _tousLesSalaries = _db.Salaries
                .Include(s => s.Site)
                .Include(s => s.Service)
                .ToList();

            DgSalaries.ItemsSource = _tousLesSalaries;

            var sites = _db.Sites.ToList();
            sites.Insert(0, new Site { Id = 0, Ville = "Tous les sites" });
            CbSite.ItemsSource = sites;
            CbSite.DisplayMemberPath = "Ville";
            CbSite.SelectedIndex = 0;

            var services = _db.Services.ToList();
            services.Insert(0, new Service { Id = 0, Nom = "Tous les services" });
            CbService.ItemsSource = services;
            CbService.DisplayMemberPath = "Nom";
            CbService.SelectedIndex = 0;
        }

        private void AppliquerFiltres()
        {
            var recherche = TxtRecherche.Text.ToLower();
            var siteSelectionne = CbSite.SelectedItem as Site;
            var serviceSelectionne = CbService.SelectedItem as Service;

            var resultats = _tousLesSalaries.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(recherche))
                resultats = resultats.Where(s =>
                    s.Nom.ToLower().Contains(recherche) ||
                    s.Prenom.ToLower().Contains(recherche));

            if (siteSelectionne != null && siteSelectionne.Id != 0)
                resultats = resultats.Where(s => s.SiteId == siteSelectionne.Id);

            if (serviceSelectionne != null && serviceSelectionne.Id != 0)
                resultats = resultats.Where(s => s.ServiceId == serviceSelectionne.Id);

            DgSalaries.ItemsSource = resultats.ToList();
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
            => AppliquerFiltres();

        private void Filtre_Changed(object sender, SelectionChangedEventArgs e)
            => AppliquerFiltres();

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TxtRecherche.Text = "";
            CbSite.SelectedIndex = 0;
            CbService.SelectedIndex = 0;
        }

        private void DgSalaries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgSalaries.SelectedItem is Salarie s)
            {
                TxtAucuneSel.Visibility = Visibility.Collapsed;
                DetailPanel.Visibility = Visibility.Visible;

                TxtInitiales.Text = $"{s.Prenom[0]}{s.Nom[0]}";
                TxtNomComplet.Text = $"{s.Prenom} {s.Nom}";
                TxtEmail.Text = s.Email;
                TxtTelFixe.Text = s.TelephoneFixe;
                TxtTelPortable.Text = s.TelephonePortable;
                TxtService.Text = s.Service?.Nom;
                TxtSite.Text = s.Site?.Ville;
            }
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            if (DgSalaries.SelectedItem is not Salarie salarie) return;

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Fiche_{salarie.Nom}_{salarie.Prenom}",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    PdfService.GenererFicheSalarie(salarie, dialog.FileName);
                    MessageBox.Show($"PDF généré avec succès !\n{dialog.FileName}",
                        "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    LogService.LogError("Export PDF", ex);
                    MessageBox.Show($"Erreur lors de la génération du PDF : {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Ctrl + Shift + A => ouvre le panneau admin
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A &&
                Keyboard.IsKeyDown(Key.LeftCtrl) &&
                Keyboard.IsKeyDown(Key.LeftShift))
            {
                var adminWindow = new Views.AdminLoginWindow();
                adminWindow.ShowDialog();
            }
        }
    }
}