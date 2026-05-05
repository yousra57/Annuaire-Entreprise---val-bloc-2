namespace AnnuaireEntreprise.Models
{
    public class Site // classe site
    {
        public int Id { get; set; }
        public string Ville { get; set; } = string.Empty;
        public ICollection<Salarie> Salaries { get; set; } = new List<Salarie>(); // un site peut avoir plusieurs salariés
    }
}