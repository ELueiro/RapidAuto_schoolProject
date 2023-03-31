using Newtonsoft.Json;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System.Text;

namespace RapidAuto.MVC.Services
{
    public class FichierMVCServiceProxy : IFichierMVCService
    {
        private readonly ILogger<FichierMVCServiceProxy> _logger;
        private readonly HttpClient _httpClient;
        private const string _fichierApiUrl = "api/Image/";

        public FichierMVCServiceProxy(HttpClient httpClient, ILogger<FichierMVCServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;    
        }

        public async Task<HttpResponseMessage> Ajouter(Fichier fichier)
        {
            
            using MultipartFormDataContent multiForm = new MultipartFormDataContent();

            multiForm.Add(new StringContent(fichier.codeVehicule), "CodeVehicule");
            multiForm.Add(new StreamContent(fichier.image1.OpenReadStream()), "Image1", "imagentest1.jpg");
            multiForm.Add(new StreamContent(fichier.image2.OpenReadStream()), "Image2", "imagentest2.jpg");

            var reponse = await _httpClient.PostAsync(_fichierApiUrl, multiForm);
            if (reponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                _logger.LogError("Fichiers images pas pu être ajoutés suite à une mauvaise requête POST de API Fichier");
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API Fichier suite à une requête POST ");
            }
            return reponse;
        }
    }
}
