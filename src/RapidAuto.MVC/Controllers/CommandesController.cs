using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System.Net;

namespace RapidAuto.MVC.Controllers
{
    public class CommandesController : Controller
    {
        private readonly ICommandeMVCService _proxy;
        private readonly IUtilisateurMVCService _proxyUtilisateur;
        private readonly IVehiculeMVCService _proxyVehicule;
        public CommandesController(ICommandeMVCService proxy, IUtilisateurMVCService proxyUtilisateur, IVehiculeMVCService proxyVehicule)
        {
            _proxy = proxy;
            _proxyUtilisateur = proxyUtilisateur;
            _proxyVehicule = proxyVehicule;
        }
        // GET: CommandesController
        public async Task<ActionResult> Index()
        {
            var commandes = await _proxy.ObtenirTout();
            var users = await _proxyUtilisateur.ObtenirTout();
            foreach (var item in commandes)
            {
                var User = users.FirstOrDefault(u=>u.Id==item.IdUtilisateur);
                item.UserNom = User.UserNom;

                var voiture = _proxyVehicule.Obtenir(item.IdVoiture);
                item.Vehicule = voiture.Result;
            }

            return View(commandes);
        }

        // GET: CommandesController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Commande commande = await _proxy.Obtenir(id.Value);
            if (commande == null)
            {
                return NotFound();
            }            
            commande.UserNom= _proxyUtilisateur.Obtenir(commande.IdUtilisateur).Result.UserNom.ToString();
            commande.Vehicule = _proxyVehicule.Obtenir(commande.IdVoiture).Result;
            return View(commande);
        }
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _proxyVehicule.Obtenir(id.Value);

            if (voiture == null)
            {
                return NotFound();
            }
            var commande = new Commande() 
            {
                    IdVoiture=id.Value,
                    Vehicule=voiture
            };

            return View(commande);
        }

        // POST: CommandesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string userNom, int id)
        {
            //On constitue la requête qui sera envoyée au service            
            if (!ExistUser(userNom))
            {
                ModelState.AddModelError("Erreur du service", "Utilisateur n'existe pas");
                return Redirect("https://localhost:7299/Utilisateurs/Create");
            }
            
            var IdUtilisateur = _proxyUtilisateur.ObtenirTout().Result.Find(u => u.UserNom == userNom).Id;

            var voitureChoisi = _proxyVehicule.Obtenir(id).Result;

            var commande = new Commande()
            {
                IdUtilisateur=IdUtilisateur,
                IdVoiture = id,
                UserNom=userNom,
                Vehicule = voitureChoisi
            };

            InformationDemandeCommande informationDemandeAEnvoye = new InformationDemandeCommande
                {
                    IdUtilisateur= commande.IdUtilisateur,
                    IdVoiture = commande.IdVoiture,
                    UserIdentity= userNom
                };

           
                var response = await _proxy.Ajouter(informationDemandeAEnvoye);

            //On valide si la création a été effectuée correctmement avant de rediriger l'utilisateur
            if (response.StatusCode == HttpStatusCode.Created) {
                voitureChoisi.Dispo = false;
                _proxyVehicule.Modifier(voitureChoisi);
                return RedirectToAction(nameof(Index));
            }
            //Sinon on affiche le message retourné par le service
            else
            {
                ModelState.AddModelError("Erreur du service", await response.Content.ReadAsStringAsync());


            }
            
            return View();
        }

        private async Task<Vehicule> ExistVoiture(int idVoiture)
        {
            var voiture =  _proxyVehicule.ObtenirTout().Result.First(u => u.Id == idVoiture);

            return voiture;

        }
        private bool ExistUser(string usernom)
        {
            var users = _proxyUtilisateur.ObtenirTout().Result.Where(u => u.UserNom== usernom).ToList();
           
            if (users.Count>0) 
            { 
                return true;
            }
            return false;
        }
        

        // GET: CommandesController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _proxy.Obtenir(id.Value);
            commande.Vehicule = await _proxyVehicule.Obtenir(commande.IdVoiture);
            commande.UserNom = _proxyUtilisateur.Obtenir(commande.IdUtilisateur).Result.UserNom;
            if (commande == null)
            {
                return NotFound();
            }
            return View(commande); 
        }

        // POST: CommandesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Commande commande)
        { 
            if (id != commande.Id)
            {
                return NotFound();
            }
            //On constitue la requête qui sera envoyée au service            
            if (!ExistUser(commande.UserNom))
            {
                ModelState.AddModelError("Erreur du service", "Utilisateur n'existe pas");
                return Redirect("https://localhost:7299/Utilisateurs/Create");
            }            
            var vehicule = await _proxyVehicule.Obtenir(commande.IdVoiture);
            var commandeAModifier = _proxy.Obtenir(id).Result;
            if (commandeAModifier.IdVoiture != vehicule.Id)
            {
                if (vehicule != null && vehicule.Dispo == true)
                {
                    commande.Vehicule = vehicule;
                    commande.Vehicule.Dispo = false;

                    var voitureAModifier = _proxyVehicule.Obtenir(commandeAModifier.IdVoiture).Result;
                    voitureAModifier.Dispo = true;
                    await _proxyVehicule.Modifier(voitureAModifier);
                   
                }
            }
            var IdUtilisateur = _proxyUtilisateur.ObtenirTout().Result.Find(u => u.UserNom == commande.UserNom).Id;
            commande.IdUtilisateur = IdUtilisateur;
            await _proxy.Modifier(commande);
            return RedirectToAction(nameof(Index));


            return View(commande);
        }


        // GET: CommandesController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _proxy.Obtenir(id.Value);

            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: CommandesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var commande =  _proxy.Obtenir(id).Result;
            commande.Vehicule = _proxyVehicule.Obtenir(commande.IdVoiture).Result;
            commande.Vehicule.Dispo = true;
            await _proxyVehicule.Modifier(commande.Vehicule);
            await _proxy.Supprimer(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
