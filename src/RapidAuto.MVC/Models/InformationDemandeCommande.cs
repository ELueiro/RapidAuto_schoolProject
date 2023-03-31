using System.ComponentModel.DataAnnotations;

namespace RapidAuto.MVC.Models
{
    public class InformationDemandeCommande
    {
        public int IdVoiture { get; set; }
        public int IdUtilisateur{ get; set; }
        [Required(ErrorMessage = "You must provide a phone number")]
        public string UserIdentity { get; set; }
    }
}
