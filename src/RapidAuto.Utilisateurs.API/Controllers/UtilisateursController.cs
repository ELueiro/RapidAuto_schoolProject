using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidAuto.Utilisateurs.API.Interfaces;
using RapidAuto.Utilisateurs.API.Models;
using RapidAuto.Utilisateurs.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RapidAuto.Utilisateurs.API.Controllers
{  

    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly IUtilisateurService _utilisateursService;
        private readonly ILogger<UtilisateursController> _logger;
        public UtilisateursController(IUtilisateurService utilisateursService, ILogger<UtilisateursController> logger)
        {
            _utilisateursService = utilisateursService;
            _logger = logger;
        }

        // GET: api/<UtilisateursController>
        /// <summary>
        /// Obtenir tout les utilisateurs
        /// </summary>
        /// <response code="200" >Liste des utilisateurs obtenue avec succès</response>
        /// <response code = "500" > Le service est indisponible pour le moment</response>
        /// <returns>Liste des utilisateurs</returns>          
        [HttpGet]
        public ActionResult<IEnumerable<Utilisateur>> Get()
        {
            return _utilisateursService.ObtenirTout().ToList();
        }

        /// <summary>
        /// Retourne un compte specifique à partir de son id
        /// </summary>
        /// <param name="id">id de l'utilisateur à retourner</param>   
        /// <response code="200">Utilisateur trouvé et retourné</response>
        /// <response code="404">Utilisateur introuvable pour l'id specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET api/<ComptesController>/5
        [HttpGet("{id}")]
        public ActionResult<Utilisateur> Get(int id)
        {
            var utilisateur = _utilisateursService.Obtenir(id);

            if (utilisateur == null)
                throw new ArgumentException(

                    $"L'utilisateur avec id: {id} n'existe pas.", nameof(id));


            return utilisateur;
        }

        /// <summary>
        /// Demande d'un nouveau compte de compte
        /// </summary>
        /// <response code="201">Utilisateur de compte ajouté</response>
        /// <response code = "400" >Informations de la demande invalides</response>
        /// <response code = "500" >Le service est indisponible pour le moment</response>
        // POST api/<UtilisateursController>
        [HttpPost]
        public IActionResult Post(Utilisateur utilisateur)
        {            
            if (ModelState.IsValid)
            {
                
                //Enregistrer l'entité Utilisateur
                var listeUserName=_utilisateursService.ObtenirTout().Select(u=>u.UserNom).ToList();
                if (listeUserName.Contains(utilisateur.UserNom))
                {
                    return BadRequest();
                }
                _utilisateursService.Ajouter(utilisateur);

                //Retourner l'entité Utilisateur créée dams la réponse HTTP
                return CreatedAtAction(nameof(Post), new { id = utilisateur.Id }, utilisateur);
            }
            // Retouner BadRequest en cas d'une erreur de validation
            LogModelState();
            return BadRequest(ModelState);
        }

        // PUT api/<utilisateursController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                LogModelState();
                return BadRequest(ModelState);
            }
            if (id != utilisateur.Id)
                return BadRequest();

            var utilisateurExistant = _utilisateursService.Obtenir(id);
            if (utilisateurExistant is null)
                return NotFound();
            if (utilisateurExistant.UserNom != utilisateur.UserNom)
            {
                var listeUserName = _utilisateursService.ObtenirTout().Select(u => u.UserNom).ToList();
                if (listeUserName.Contains(utilisateur.UserNom))
                {
                    return BadRequest();
                }
            }

            try
            {
                _utilisateursService.Modifier(utilisateur);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError($"Une exception s'est produite : {ex.Message}");

                if (_utilisateursService.Obtenir(utilisateur.Id)==null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            
            _utilisateursService.Modifier(utilisateur);


            return NoContent();
        }

        // DELETE api/<UtilisateursController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var utilisateur = _utilisateursService.Obtenir(id);

            if (utilisateur is null)
                return NotFound();

            _utilisateursService.Supprimer(utilisateur);

            return NoContent();
        }

        private void LogModelState()
        {
            foreach (var model in ModelState)
            {
                _logger.LogInformation($"Model invalide pour la propriété {model.Key}. Message d'erreur : {model.Value.Errors[0].ErrorMessage}");
            }
        }
    }
}
