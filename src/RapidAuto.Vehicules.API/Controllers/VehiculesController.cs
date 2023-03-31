using Microsoft.AspNetCore.Mvc;
using RapidAuto.Vehicules.API.Interfaces;
using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class VehiculesController : ControllerBase
    {
        private readonly IVehiculeService _vehiculeService;

        private readonly IConfiguration _config;

        public VehiculesController(IVehiculeService vehiculesService, IConfiguration config)
        {
            _vehiculeService = vehiculesService;
            _config = config;
        }

        // GET: api/<UtilisateursController>
        /// <summary>
        /// Obtenir tout les utilisateurs
        /// </summary>
        /// <response code="200" >Liste des vehicules obtenue avec succès</response>
        /// <response code = "500" > Le service est indisponible pour le moment</response>
        /// <returns>Liste des utilisateurs</returns>          
        [HttpGet]
        public ActionResult<IEnumerable<Vehicule>> Get()
        {
            return _vehiculeService.ObtenirTout().ToList();
        }

        /// <summary>
        /// Retourne un compte specifique à partir de son id
        /// </summary>
        /// <param name="id">id de l'utilisateur à retourner</param>   
        /// <response code="200">Vehicule trouvé et retourné</response>
        /// <response code="404">Vehicule introuvable pour l'id specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET api/<ComptesController>/5
        [HttpGet("{id}")]
        public ActionResult<Vehicule> Get(int id)
        {
            var vehicule = _vehiculeService.Obtenir(id);

            if (vehicule == null)
                throw new ArgumentException(
                    $"Il n'y a pas de véhicule avac id : {id}.", nameof(id));

            return vehicule;
        }

        /// <summary>
        /// Demande d'un nouveau compte de compte
        /// </summary>
        /// <response code="201">Vehicule de compte ajouté</response>
        /// <response code = "400" >Informations de la demande invalides</response>
        /// <response code = "500" >Le service est indisponible pour le moment</response>
        // POST api/<UtilisateursController>
        [HttpPost]
        public IActionResult Post(Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                //Generation d'un NIV pour un vehicule

                //vehicule.Niv = vehicule.Constructeur + Guid.NewGuid().ToString();
                _vehiculeService.Ajouter(vehicule);
               
                //Retourner l'entité véhicule créée dams la réponse HTTP
                return CreatedAtAction(nameof(Post), new { id = vehicule.Id }, vehicule);
            }
            // Retouner BadRequest en cas d'une erreur de validation
            return BadRequest(ModelState);
        }

        // PUT api/<utilisateursController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Vehicule vehicule)
        {
            if (id != vehicule.Id)
                return BadRequest();

            var vehiculeExistant = _vehiculeService.Obtenir(id);
            if (vehiculeExistant is null)
                return NotFound();

            _vehiculeService.Modifier(vehicule);

            return NoContent();
        }

        // DELETE api/<UtilisateursController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var vehicule = _vehiculeService.Obtenir(id);

            if (vehicule is null)
                return NotFound();

            _vehiculeService.Supprimer(vehicule);

            return NoContent();
        }
    }
}
