using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interface
{
    public interface ICommandeMVCService
    {
        Task<List<Commande>> ObtenirTout();

        Task<Commande> Obtenir(int id);
        Task<HttpResponseMessage> Ajouter(InformationDemandeCommande informationDemande);
        Task Modifier(Commande commande);
        Task Supprimer(int id);
    }
}
