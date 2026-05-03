using System.Windows;

namespace AnnuaireEntreprise.Views
{ // class SimpleFormWindow : une fenêtre générique pour saisir une valeur simple (ex: nom de site/service)
    public partial class SimpleFormWindow : Window
    {
        public string Valeur => TxtValeur.Text.Trim();

        public SimpleFormWindow(string titre, string label, string valeurInitiale)
        {
            InitializeComponent();
            Title = titre;
            TxtLabel.Text = label;
            TxtValeur.Text = valeurInitiale;
        }
    // Valider ou annuler la saisie
        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtValeur.Text))
            {
                MessageBox.Show("Le champ ne peut pas être vide.", "Erreur");
                return;
            }
            DialogResult = true;
        }
    // Annuler la saisie
        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}