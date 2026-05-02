using Microsoft.EntityFrameworkCore;

namespace AnnuaireEntreprise.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            db.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");
        }
    }
}