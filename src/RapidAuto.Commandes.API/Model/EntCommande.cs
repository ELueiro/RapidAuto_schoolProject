using System.ComponentModel.DataAnnotations;

namespace RapidAuto.Commandes.API.Model
{
    public class EntCommande:BaseEntity
    {
        [Required(ErrorMessage = "Le champ est obligatoire")]
        [Display(Name = "ID de l'usager")]
        public int IdUtilisateur { get; set; }

        [Required(ErrorMessage = "Le champ est obligatoire")]
        [Display(Name = "ID de voiture")]
        public int IdVoiture { get; set; }

        [Required(ErrorMessage = "Le champ est obligatoire")]
        [Display(Name = "Date de creation de commande")]
        [DataType(DataType.Date)]
        public DateTime DateCreation { get; set; }
    }
}
