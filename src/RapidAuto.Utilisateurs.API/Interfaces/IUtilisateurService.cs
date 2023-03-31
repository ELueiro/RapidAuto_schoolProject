using RapidAuto.Utilisateurs.API.Models;

namespace RapidAuto.Utilisateurs.API.Interfaces
{
    public interface IUtilisateurService
    {
        public IEnumerable<Utilisateur> ObtenirTout();
        public Utilisateur Obtenir(int id);
        public void Ajouter(Utilisateur utilisateur);

        public void Modifier(Utilisateur utilisateur);
        public void Supprimer(Utilisateur utilisateur);
    }
}
