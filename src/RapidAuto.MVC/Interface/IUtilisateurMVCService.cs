using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interface
{
    public interface IUtilisateurMVCService
    {
        Task<List<Utilisateur>> ObtenirTout();

        Task<Utilisateur> Obtenir(int id);
        Task Ajouter(Utilisateur utilisateur);

        Task Modifier(Utilisateur utilisateur);
        Task Supprimer(int id);
    }
}
