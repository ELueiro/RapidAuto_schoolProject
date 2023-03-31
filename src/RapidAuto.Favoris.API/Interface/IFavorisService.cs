using RapidAuto.Favoris.API.Models;

namespace RapidAuto.Favoris.API.Interface
{
    public interface IFavorisService
    {
        public IEnumerable<Vehicule> ObtenirTout();
        public Vehicule Obtenir(int id);
        public void Ajouter(Vehicule vehicule);
    }
}
