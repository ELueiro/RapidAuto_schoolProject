using RapidAuto.Favoris.API.Interface;
using RapidAuto.Favoris.API.Models;

namespace RapidAuto.Favoris.API.Services
{
    public class FavorisService : IFavorisService
    {
        private readonly IRepository<Vehicule> _vehiculeFavorisRepository;
        //static int prochainId = 0;
        //public List<Vehicule>? Vehicules { get; }

        public FavorisService(IRepository<Vehicule> vehiculeRepository)
        {
            _vehiculeFavorisRepository = vehiculeRepository;
        }

        public IEnumerable<Vehicule> ObtenirTout()
        {
            return _vehiculeFavorisRepository.List();
        }

        public Vehicule Obtenir(int id)
        {
            return _vehiculeFavorisRepository.GetById(id);
        }

        public void Ajouter(Vehicule vehiculeFavoris)
        {
            //vehiculeFavoris.Id = prochainId++;
            _vehiculeFavorisRepository.Add(vehiculeFavoris);
        }
    }
}
