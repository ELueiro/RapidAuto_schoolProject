using Microsoft.AspNetCore.Mvc;
using RapidAuto.Commandes.API.Interface;
using RapidAuto.Commandes.API.Model;

namespace RapidAuto.Commandes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandesController : Controller
    {
        private readonly ICommandeService _commandesService;

        public CommandesController(ICommandeService commandesService)
        {
            _commandesService = commandesService;
        }
        // GET: api/<CommandesController>
        /// <summary>
        /// Obtenir tout les commandes
        /// </summary>
        /// <response code="200" >Liste des commandes obtenue avec succès</response>
        /// <response code = "500" > Le service est indisponible pour le moment</response>
        /// <returns>Liste des commandes</returns>          
        [HttpGet]
        public ActionResult<IEnumerable<EntCommande>> Get()
        {
            return _commandesService.ObtenirTout().ToList();
        }

        /// <summary>
        /// Retourne un commande specifique à partir de son id
        /// </summary>
        /// <param name="id">id de l'utilisateur à retourner</param>   
        /// <response code="200">Commande trouvé et retourné</response>
        /// <response code="404">Commande introuvable pour l'id specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET api/<CommandesController>/5
        [HttpGet("{id}")]
        public ActionResult<EntCommande> Get(int id)
        {
            var commande = _commandesService.Obtenir(id);

            if (commande == null)
            {
                throw new ArgumentException(
                    $"Il n'y a pas de commande avac id : {id}.", nameof(id));
            }

            return commande;
        }

        /// <summary>
        /// Demande d'un nouveau commande de voiture
        /// </summary>
        /// <param name="informationDemande">Informations sur la demande</param>
        /// <response code="201">commande ajouté</response>
        /// <response code = "400" >Informations de la demande invalides</response>
        /// <response code = "500" >Le service est indisponible pour le moment</response>
        // POST api/<CommandesController>
        [HttpPost]
        public IActionResult Post([FromBody] InformationDemandeCommande informationDemande)
        {
            if (ModelState.IsValid)
            {                
                EntCommande commande = new EntCommande
                {
                    DateCreation = DateTime.Now,
                    IdUtilisateur = informationDemande.IdUtilisateur,
                    IdVoiture = informationDemande.IdVoiture
                    
                };

                //Enregistrer l'entité commande
                _commandesService.Ajouter(commande);

                //Retourner l'entité commande créée dams la réponse HTTP
                return CreatedAtAction(nameof(Post), new { id = commande.Id }, commande);
            }
            // Retouner BadRequest en cas d'une erreur de validation
            return BadRequest(ModelState);
        }

        // PUT api/<CommandesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, EntCommande commande)
        {
            if (id != commande.Id)
                return BadRequest();

            var commandeExistant = _commandesService.Obtenir(commande.Id);
            if (commandeExistant is null)
                return NotFound();
            commande.DateCreation = commandeExistant.DateCreation;
            
            _commandesService.Modifier(commande);

            return NoContent();
        }

        // DELETE api/<CommandesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var commande = _commandesService.Obtenir(id);

            if (commande is null)
                return NotFound();

            _commandesService.Supprimer(commande);

            return NoContent();
        }
    }
}
