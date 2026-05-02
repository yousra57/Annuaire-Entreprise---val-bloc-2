namespace AnnuaireEntreprise.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Ville { get; set; } = string.Empty;
        public ICollection<Salarie> Salaries { get; set; } = new List<Salarie>();
    }
}