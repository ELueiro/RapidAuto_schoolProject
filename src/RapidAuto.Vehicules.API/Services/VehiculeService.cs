using RapidAuto.Vehicules.API.Interfaces;
using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Services
{
    public class VehiculeService : IVehiculeService
    {
        private readonly IRepository<Vehicule> _vehiculeRepository;
        
        public List<Vehicule> Vehicules { get; }

        public VehiculeService(IRepository<Vehicule> VehiculeRepository)
        {
            _vehiculeRepository = VehiculeRepository;
        }

        public IEnumerable<Vehicule> ObtenirTout()
        {
            return _vehiculeRepository.List();
        }

        public Vehicule Obtenir(int id)
        {
            return _vehiculeRepository.GetById(id);
        }
        public void Ajouter(Vehicule Vehicule)
        {
           
            _vehiculeRepository.Add(Vehicule);
        }

        public void Supprimer(Vehicule Vehicule)
        {
            var VehiculeASupprimer = Obtenir(Vehicule.Id);
            if (VehiculeASupprimer is null)
                return;

            _vehiculeRepository.Delete(Vehicule);
        }

        public void Modifier(Vehicule Vehicule)
        {
            var VehiculeAModifier = Obtenir(Vehicule.Id);
            if (VehiculeAModifier is null)
                return;

            _vehiculeRepository.Edit(Vehicule);
        }
    }
}
