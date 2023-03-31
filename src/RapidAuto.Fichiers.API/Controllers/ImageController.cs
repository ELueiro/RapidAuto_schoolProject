using Microsoft.AspNetCore.Mvc;
using RapidAuto.Fichiers.API.Interfaces;
using RapidAuto.Fichiers.API.Models;

namespace RapidAuto.Fichiers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageService _imagesService;

        public ImageController(IImageService imagesService)
        {
            _imagesService = imagesService;
        }

        /// <summary>
        /// save les images
        /// </summary>
        /// <response code="201">Utilisateur de compte ajouté</response>
        /// <response code = "400" >Informations de la demande invalides</response>
        /// <response code = "500" >Le service est indisponible pour le moment</response>
        // POST api/<UtilisateursController>
        [HttpPost]
        
        public async Task<ActionResult> Post([FromForm] Fichier images)
            
        {
            var result = await _imagesService.SauvegarderImages(images);
            return CreatedAtAction("Post", new ImagesNom {ImageName1= result[0], ImageName2= result[1]});

        }  
    }
}
