using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Controllers
{
    public class UtilisateursController : Controller
    {
        //private readonly IConfiguration _config;

        private readonly ILogger<UtilisateursController> _logger;
        private readonly IUtilisateurMVCService _proxy;
        public UtilisateursController( IUtilisateurMVCService proxy, ILogger<UtilisateursController> logger)
        {
            _proxy = proxy;
            _logger = logger;
            
        }
        // GET: UtilisateursController

        //La consultation est accessible aux personnes ayant le rôle Utilisateur ou Administrateur
       
        public async Task<ActionResult> Index()
        {   
            var users= await _proxy.ObtenirTout();
            _logger.LogInformation(CustomLogEvents.Lecture, $"Obtention de {users.Count} users");
            return View(users);
        }



        // GET: UtilisateursController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var utilisateur = await _proxy.Obtenir(id.Value);

            if (utilisateur == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Affichage du utilisateur ayant pour ID {utilisateur.Id}");
            return View(utilisateur);
        }

        // GET: UtilisateursController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: UtilisateursController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,UserNom,Nom,Prenom,Courriel,NumeroTel")] Utilisateur utilisateur)
        {
            //On obtient l'identifiant de l'utilisateur connectée via le UserManager
                     

            if (ModelState.IsValid)
            {                
                await _proxy.Ajouter(utilisateur);
                _logger.LogInformation(CustomLogEvents.Creation, $"Enregistrement de Utilisateurs avec ID {utilisateur.Id}");
                return RedirectToAction(nameof(Index));
            }

            return View(utilisateur);

        }

        // GET: UtilisateursController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _proxy.Obtenir(id.Value);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        // POST: UtilisateursController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,UserNom,Nom,Prenom,Courriel,NumeroTel")]  Utilisateur utilisateur)
        {
            if (id != utilisateur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _proxy.Modifier(utilisateur);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_proxy.Obtenir(utilisateur.Id)==null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(utilisateur);
        }

        // GET: UtilisateursController/Delete/5
        public async Task<ActionResult> Supprimer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _proxy.Obtenir(id.Value);

            if (utilisateur == null)
            {
                return NotFound();
            }
            
            return View(utilisateur);
        }

        // POST: UtilisateursController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Supprimer(int id)
        {
            await _proxy.Supprimer(id);
            return RedirectToAction(nameof(Index));

        }
        
    }
}
