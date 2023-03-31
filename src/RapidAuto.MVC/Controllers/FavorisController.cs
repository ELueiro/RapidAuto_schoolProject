using Microsoft.AspNetCore.Mvc;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Controllers
{
    public class FavorisController : Controller
    {
        private readonly IFavorisMVCService _proxy;
        private readonly IVehiculeMVCService _proxyVehicule;
        private readonly IConfiguration _config;

        public FavorisController(IVehiculeMVCService proxyVehicule , IFavorisMVCService proxy, IConfiguration config)
        {
            _proxy = proxy;
            _proxyVehicule = proxyVehicule;
            _config = config;
        }
        public async  Task<IActionResult> Index()
        {
            var listeDeVehicules = await _proxy.ObtenirTout();
            return View(listeDeVehicules);
        }
       

        public async  Task<ActionResult> Create(int id) 
        {
            var vehiculeAAjouterDansFavoris = await  _proxyVehicule.Obtenir(id);
            await _proxy.Ajouter(vehiculeAAjouterDansFavoris);
            return RedirectToAction(nameof(Index));

        }

        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(Vehicule vehicule)
        //{
        //    await _proxy.Ajouter(vehicule);
        //    return RedirectToAction(nameof(Index));

        //}
    }
}
