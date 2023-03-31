using RapidAuto.Utilisateurs.API.Interfaces;
using RapidAuto.Utilisateurs.API.Models;

namespace RapidAuto.Utilisateurs.API.Services
{
    public  class UtilisateurService : IUtilisateurService

    {
        private readonly IRepository<Utilisateur> _utilisateurRepository;
        static int prochainId = 0;
        public List<Utilisateur> Utilisateurs { get; }

        public UtilisateurService(IRepository<Utilisateur> utilisateurRepository)
        {
            _utilisateurRepository = utilisateurRepository;
        }

        public IEnumerable<Utilisateur> ObtenirTout()
        {
            return _utilisateurRepository.List();
        }

        public Utilisateur Obtenir(int id)
        {
            return _utilisateurRepository.GetById(id);
        }
        public void Ajouter(Utilisateur utilisateur)
        {
            utilisateur.Id = prochainId++;
            _utilisateurRepository.Add(utilisateur);
        }

        public  void Supprimer(Utilisateur utilisateur)
        {
            var utilisateurASupprimer = Obtenir(utilisateur.Id);
            if (utilisateurASupprimer is null)
                return;

            _utilisateurRepository.Delete(utilisateur);
        }

        public  void Modifier(Utilisateur utilisateur)
        {
            var utilisateurAModifier = Obtenir(utilisateur.Id);
            if (utilisateurAModifier is null)
                return;
            _utilisateurRepository.Delete(utilisateurAModifier);
            _utilisateurRepository.Add(utilisateur);
        }
        
    }
}

