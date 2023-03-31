using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interface
{
    public interface IVehiculeMVCService
    {
        Task<List<Vehicule>> ObtenirTout();

        Task<Vehicule> Obtenir(int id);
        Task Ajouter(Vehicule vehicule);

        Task Modifier(Vehicule vehicule);
        Task Supprimer(int id);
    }
}
