using RapidAuto.Fichiers.API.Interfaces;
using RapidAuto.Fichiers.API.Models;
using System.IO;

namespace RapidAuto.Fichiers.API.Services
{
    public class ImageService: IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironement;
        
        public ImageService(IWebHostEnvironment env)
        {
            _webHostEnvironement = env;
        }
        public async Task<List<string>> SauvegarderImages(Fichier images)
        {

            List<string> imagesName = new List<string>();
            string img1Name = $"I1{images.codeVehicule}.png";
            string img2Name = $"I2{images.codeVehicule}.png";
            var path = _webHostEnvironement.WebRootPath + @"\..\..\RapidAuto.MVC\wwwroot\images\" + img1Name;
            var path2 = _webHostEnvironement.WebRootPath + @"\..\..\RapidAuto.MVC\wwwroot\images\" + img2Name;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await images.image1.CopyToAsync(stream);
            }
            using (var stream = new FileStream(path2, FileMode.Create))
            {
                await images.image2.CopyToAsync(stream);
            }

            //On retourne le nom de l'image
            imagesName.Add(img1Name);
            imagesName.Add(img2Name);
            return imagesName;
        }

    }
}
