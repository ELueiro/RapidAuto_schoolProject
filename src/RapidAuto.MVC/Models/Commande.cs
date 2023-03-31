using System.ComponentModel.DataAnnotations;

namespace RapidAuto.MVC.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public Vehicule Vehicule { get; set; }
        public int IdVoiture { get; set; }
        public int IdUtilisateur { get; set; }
        [Required(ErrorMessage = "Le champ est obligatoire")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "La taille maximale du champ est de 50 caractères")]
        public string UserNom { get; set; }
    }
}
