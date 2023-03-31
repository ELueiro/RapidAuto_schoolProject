using System.ComponentModel.DataAnnotations;
namespace RapidAuto.MVC.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le champ est obligatoire")]
        [DataType(DataType.Text)]
        [MaxLength(10, ErrorMessage = "La taille maximale du champ est de 10 caractères")]
        public string UserNom { get; set; }

        [Required(ErrorMessage = "Le champ est obligatoire")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "La taille maximale du champ est de 50 caractères")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le champ est obligatoire")]
        [DataType(DataType.Text)]
        [Display(Name = "Prénom")]
        [MaxLength(50, ErrorMessage = "La taille maximale du champ est de 50 caractères")]
        public string Prenom { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Courriel { get; set; }

        [Required(ErrorMessage = "Le champ est obligatoire")]
        [Display(Name = "Numéro de téléphone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(([0-9]{3}))[-]([0-9]{3})[-]([0-9]{4})$")]
        public string NumeroTel { get; set; }
    }
}
