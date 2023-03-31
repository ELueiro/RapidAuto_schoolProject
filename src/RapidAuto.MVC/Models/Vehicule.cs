using System.ComponentModel.DataAnnotations;

namespace RapidAuto.MVC.Models
{
    public class Vehicule
    {
        public int? Id { get; set; }
        [Required]
        public string Constructeur { get; set; }
        [Required]
        public string? Modele { get; set; }
        [Required]
        public int? AnneeFabrication { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public int? NbSieges { get; set; }
        [Required]
        public string? Couleur { get; set; }
        public string? Niv { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Description { get; set; }
        public bool Dispo { get; set; }
        [Required]
        public decimal? Prix { get; set; }

    }
}
