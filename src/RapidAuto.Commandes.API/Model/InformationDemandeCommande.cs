using System.ComponentModel.DataAnnotations;

namespace RapidAuto.Commandes.API.Model
{
    public class InformationDemandeCommande
    {
        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public int IdVoiture { get; set; }


        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public int IdUtilisateur { get; set; }
    }
}
