namespace AnnuaireEntreprise.Models
{
    public class Service // classe service
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public ICollection<Salarie> Salaries { get; set; } = new List<Salarie>();
    }
}