using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Interfaces
{
    public interface IVehiculeService
    {
        public IEnumerable<Vehicule> ObtenirTout();
        public Vehicule Obtenir(int id);
        public void Ajouter(Vehicule vehicule);

        public void Modifier(Vehicule vehicule);
        public void Supprimer(Vehicule vehicule);
    }


}
