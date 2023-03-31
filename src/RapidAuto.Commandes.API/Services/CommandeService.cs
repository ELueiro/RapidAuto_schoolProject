using RapidAuto.Commandes.API.Interface;
using RapidAuto.Commandes.API.Model;

namespace RapidAuto.Commandes.API.Services
{
    public class CommandeService: ICommandeService
    {
        private readonly IRepository<EntCommande> _commandeRepository;
        static int prochainId = 0;
        public List<EntCommande> Commandes { get; }

        public CommandeService(IRepository<EntCommande> commandeRepository)
        {
            _commandeRepository = commandeRepository;
        }

        public IEnumerable<EntCommande> ObtenirTout()
        {
            return _commandeRepository.List();
        }

        public EntCommande Obtenir(int id)
        {
            return  _commandeRepository.GetById(id);
        }
        public void Ajouter(EntCommande commande)
        {
            commande.Id = prochainId++;
            _commandeRepository.Add(commande);
        }

        public void Supprimer(EntCommande commande)
        {
            var commandeASupprimer = Obtenir(commande.Id);
            if (commandeASupprimer is null)
                return;

            _commandeRepository.Delete(commande);
        }

        public void Modifier(EntCommande commande)
        {
            var commandeAModifier = Obtenir(commande.Id);
            if (commandeAModifier is null)
                return;
            _commandeRepository.Delete(commandeAModifier);
            _commandeRepository.Add(commande);
        }
    }
}
