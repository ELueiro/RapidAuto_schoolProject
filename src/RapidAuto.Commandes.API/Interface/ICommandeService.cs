using RapidAuto.Commandes.API.Model;

namespace RapidAuto.Commandes.API.Interface
{
    public interface ICommandeService
    {
        public IEnumerable<EntCommande> ObtenirTout();
        public EntCommande Obtenir(int id);
        public void Ajouter(EntCommande Commande);

        public void Modifier(EntCommande Commande);
        public void Supprimer(EntCommande Commande);
    }
}
