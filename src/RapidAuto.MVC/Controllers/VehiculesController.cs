using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using RapidAuto.MVC.Services;

namespace RapidAuto.MVC.Controllers
{
    public class VehiculesController : Controller
    {
        private readonly ILogger<VehiculesController> _logger;
        private readonly IVehiculeMVCService _proxy;
        private readonly IConfiguration _config;
        private readonly IFichierMVCService _fichierProxy;

        public VehiculesController(IVehiculeMVCService proxy, IConfiguration config, IFichierMVCService proxyFichier, ILogger<VehiculesController> logger)
        {
            _logger = logger;
            _proxy = proxy;
            _config = config;
            _fichierProxy = proxyFichier;
        }
        public async Task<IActionResult> Index(string OrdreDeTri, string recherche, string filtre)
        {
            var listeDeVehicules = await _proxy.ObtenirTout();

            ViewBag.PrixOrdreDeTriParm = String.IsNullOrEmpty(OrdreDeTri) ? "prix_desc" : "";

            if (!String.IsNullOrEmpty(recherche))
            {
                listeDeVehicules = listeDeVehicules.Where(v => v.Constructeur.Contains(recherche) || v.Modele.Contains(recherche) ).ToList();
                _logger.LogInformation(ActionsUtilisateursLogEvents.Recherche,"Recherche d'un vehicule avec le mot {0}", recherche);
            }

            if (!String.IsNullOrEmpty(filtre))
            {
                listeDeVehicules = listeDeVehicules.Where(v => v.Constructeur.ToLower() == filtre.ToLower() ).ToList();
            }


            switch (OrdreDeTri)
            {
                case "prix_desc":
                    listeDeVehicules = listeDeVehicules.OrderByDescending(v => v.Prix).ToList();
                    break;
                default:
                    listeDeVehicules = listeDeVehicules.OrderBy(v => v.Prix).ToList();
                    break;
            }
            
            if (listeDeVehicules != null)
            { 
                return View(listeDeVehicules);

            }
            else return NotFound();

        }

        // GET: VehiculesController/Create
        public  IActionResult Create()
        {
            //ViewBag.TypeDeVehicule = ("Hybrid", "Gas", "Electric");
            ViewBag.TypeDeVehicule = _config.GetSection("TypeDeVehicule").Get<string[]>().ToList(); 
            return View();
        }

        // POST:VehiculesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Vehicule vehicule, IFormFile image1, IFormFile image2)
        {
            ViewBag.TypeDeVehicule = ("Hybrid", "Gas", "Electric");
            //ViewBag.TypeDeVehicule = _config.GetSection("TypeDeVehicule").Get<string[]>().ToList();
            if (image1 == null || image2 == null)
            {
                ViewBag.ErrorMessage = "Images Obligatoires";
                return View();
            }

            //On obtient l'identifiant de l'utilisateur connectée via le UserManager
            vehicule.Id = 0;
            Random random = new Random();
            vehicule.Niv = vehicule.Constructeur + random.Next(10000, 99999);
            Fichier fichier = new();
            fichier.image1 = image1;
            fichier.image2 = image2;
            fichier.codeVehicule = vehicule.Niv;                     

            var reponse =  await _fichierProxy.Ajouter(fichier);

            if (reponse.IsSuccessStatusCode)
            {
                var imagesNom = await reponse.Content.ReadFromJsonAsync<ImagesNom>();
                vehicule.Image1 = imagesNom.ImageName1;
                vehicule.Image2 = imagesNom.ImageName2;

            }


            if (ModelState.IsValid)
            {
                await _proxy.Ajouter(vehicule);
                return RedirectToAction(nameof(Index));
            }

            return View(vehicule);

        }

        // GET: VehiculesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            var vehicule= await _proxy.Obtenir(id);

            if (vehicule == null)
            {
                return NotFound();
            }

            return View(vehicule);
        }

        // GET: VehiculesController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicule = await _proxy.Obtenir(id.Value);

            if (vehicule == null)
            {
                return NotFound();
            }

            return View(vehicule);
        }

        // POST: VehiculesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Vehicule vehicule)
        {
            if (id != vehicule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _proxy.Modifier(vehicule);
                return RedirectToAction(nameof(Index));
            }

            return View(vehicule);
        }

        // GET: VehiculesController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicule = await _proxy.Obtenir(id.Value);

            if (vehicule == null)
            {
                return NotFound();
            }

            return View(vehicule);
        }

        // POST: VehiculesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {

            await _proxy.Supprimer(id);
            return RedirectToAction(nameof(Index));

        }


    }
}
