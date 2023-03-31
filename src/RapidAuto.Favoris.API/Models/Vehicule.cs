namespace RapidAuto.Favoris.API.Models
{
    public class Vehicule : BaseEntity
    {
        public string Constructeur { get; set; }
        public string Modele { get; set; }
        public int AnneeFabrication { get; set; }
        public string Type { get; set; }
        public int NbSieges { get; set; }
        public string Couleur { get; set; }
        public string Niv { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Description { get; set; }
        public bool Dispo { get; set; }
        public decimal Prix { get; set; }
    }
}
