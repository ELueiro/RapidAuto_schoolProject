using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interface
{
    public interface IFavorisMVCService
    {
        Task<List<Vehicule>> ObtenirTout();
        Task Ajouter(Vehicule vehicule);
    }
}
