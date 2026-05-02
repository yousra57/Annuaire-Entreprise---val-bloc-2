using System.IO;

namespace AnnuaireEntreprise.Services
{
    public static class LogService
    {
        private static readonly string LogPath = "logs_admin.txt";

        public static void LogAdminAccess(string action)
        {
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ADMIN] {action}";
            File.AppendAllText(LogPath, line + Environment.NewLine);
        }

        public static void LogError(string context, Exception ex)
        {
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERREUR] {context} : {ex.Message}";
            File.AppendAllText(LogPath, line + Environment.NewLine);
        }
    }
}