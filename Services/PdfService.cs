using AnnuaireEntreprise.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AnnuaireEntreprise.Services
{ 
    public static class PdfService // classe pour générer une fiche PDF d'un salarié, utilisé MainWindow 
    {
        public static void GenererFicheSalarie(Salarie salarie, string cheminFichier) //méthode pour générer la fiche PDF d'un salarié, appelée depuis MainWindow lors du clic sur le bouton d'export
        {
            QuestPDF.Settings.License = LicenseType.Community;

    // SLIDE 3 - GÉNÉRATION DE PDF AVEC QUESTPDF
            Document.Create(container =>
            {
                container.Page(page => // mise en page du PDF
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header().Background("#fab8b8").Padding(15).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("📋 Annuaire Entreprise")
                                .FontColor("#FFFFFF").FontSize(20).Bold();
                            col.Item().Text("Fiche Salarié")
                                .FontColor("#BDC3C7").FontSize(13);
                        });
                    });

                    // Contenu
                    page.Content().Padding(20).Column(col =>
                    {
                        // Nom complet du salarié avec service et site, dans une boîte grisée
                        col.Item().Background("#ECF0F1").Padding(15).Row(row =>
                        {
                            row.RelativeItem().Column(inner =>
                            {
                                inner.Item().Text($"{salarie.Prenom} {salarie.Nom}")
                                    .FontSize(22).Bold().FontColor("#600000");
                                inner.Item().Text($"{salarie.Service?.Nom} — {salarie.Site?.Ville}")
                                    .FontSize(13).FontColor("#7F8C8D");
                            });
                        });

                        col.Item().Height(20);

                        // Informations de contact dans une table avec deux colonnes : label et valeur
                        col.Item().Text("Informations de contact")
                            .FontSize(15).Bold().FontColor("#600000");

                        col.Item().Height(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(160);
                                columns.RelativeColumn();
                            });

                            // Ligne helper = pour éviter de répéter le même code pour chaque ligne d'information
                            void AjouterLigne(string label, string valeur)
                            {
                                table.Cell().Background("#F8F9FA").Padding(8)
                                    .Text(label).Bold().FontColor("#555555");
                                table.Cell().BorderBottom(1).BorderColor("#EEEEEE")
                                    .Padding(8).Text(valeur);
                            }

                            AjouterLigne("Email", salarie.Email);
                            AjouterLigne("Téléphone fixe", salarie.TelephoneFixe);
                            AjouterLigne("Téléphone portable", salarie.TelephonePortable);
                            AjouterLigne("Service", salarie.Service?.Nom ?? "-");
                            AjouterLigne("Site", salarie.Site?.Ville ?? "-");
                        });

                        col.Item().Height(30);

                        // Date de génération
                        col.Item().Text($"Document généré le {DateTime.Now:dd/MM/yyyy à HH:mm}")
                            .FontSize(10).FontColor("#95A5A6").Italic();
                    });

                    // Footer = bas de page avec un texte centré
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Annuaire Entreprise — Document confidentiel")
                            .FontSize(10).FontColor("#95A5A6");
                    });
                });
            }).GeneratePdf(cheminFichier);
        }
    }
}