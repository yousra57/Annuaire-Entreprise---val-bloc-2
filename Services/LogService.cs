using System.IO;
// fichier pour gérer les logs d'accès admin et les erreurs
namespace AnnuaireEntreprise.Services
{
    public static class LogService
    {
        private static readonly string LogPath = "logs_admin.txt"; // chemin pour générer le fichier de logs 
    // SLIDE 7 - Service de logging pour les accès admin et les erreurs
        public static void LogAdminAccess(string action) // méthode pour enregistrer les accès admin (ex: "Connexion réussie" ou "Tentative de connexion échouée")
        {
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ADMIN] {action}";
            File.AppendAllText(LogPath, line + Environment.NewLine);
        }

        public static void LogError(string context, Exception ex) //méthode en cas d'erreur, pour enregistrer le contexte de l'erreur et le message de l'exception
        {
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERREUR] {context} : {ex.Message}";
            File.AppendAllText(LogPath, line + Environment.NewLine);
        }
    }
}