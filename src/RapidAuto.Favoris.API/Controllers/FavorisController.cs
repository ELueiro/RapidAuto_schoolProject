using Microsoft.AspNetCore.Mvc;
using RapidAuto.Favoris.API.Interface;
using RapidAuto.Favoris.API.Models;

namespace RapidAuto.Favoris.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private readonly IFavorisService _favorisService;

        
        public FavorisController(IFavorisService favorisService)
        {
            _favorisService = favorisService;
        }
        // GET: api/<FavorisController>
        /// <summary>
        /// Obtenir tout les vehicules Favoris
        /// </summary>
        /// <response code="200" >Liste des vehicules Favoris obtenue avec succès</response>
        /// <response code = "500" > Le service est indisponible pour le moment</response>
        /// <returns>Liste des vehicules Favoris</returns>          
        [HttpGet]
        public ActionResult<IEnumerable<Vehicule>> Get()
        {
            return _favorisService.ObtenirTout().ToList();
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
        public ActionResult<Vehicule> GetById(int id)
        {
            var vehicule = _favorisService.Obtenir(id);

            if (vehicule == null)
            {
                throw new ArgumentException(
                    $"Il n'y a pas le voiture  avac id : {id}. dans la list favoris", nameof(id));
            }

            return vehicule;
            
        }

        /// <summary>
        /// Ajout d'un vehicule aux favoris
        /// </summary>
        /// <param name="vehicule">vehicule à ajouter dans les favoris</param>
        /// <response code="201">vehicule ajouté</response>
        /// <response code = "400" >vehicule invalide</response>
        /// <response code = "500" >Le service est indisponible pour le moment</response>
        // POST api/<FavorisController>
        [HttpPost]
        public IActionResult Post(Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
              
                //Enregistrer l'entité vehicule
                _favorisService.Ajouter(vehicule);

                //Retourner l'entité commande créée dams la réponse HTTP
                return CreatedAtAction(nameof(Post), new { id = vehicule.Id }, vehicule);
            }
            // Retouner BadRequest en cas d'une erreur de validation
            return BadRequest(ModelState);
        }
    }
}
