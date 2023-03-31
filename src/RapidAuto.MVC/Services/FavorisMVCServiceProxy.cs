using Newtonsoft.Json;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System.Text;

namespace RapidAuto.MVC.Services
{
    public class FavorisMVCServiceProxy : IFavorisMVCService
    {
        private readonly ILogger<FavorisMVCServiceProxy> _logger;
        private readonly HttpClient _httpClient;
        private const string _apiUrl = "api/Favoris/";

        public FavorisMVCServiceProxy(HttpClient httpClient, ILogger<FavorisMVCServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task Ajouter(Vehicule vehicule)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(vehicule), Encoding.UTF8, "application/json");
            var reponse = await _httpClient.PostAsync(_apiUrl, content);
            if (reponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                _logger.LogError("Vehicule avec ID {0} n'a pas pu être ajouté suite à une mauvaise requête POST de API Favoris", vehicule.Id);
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API Favoris suite à une requête POST ");
            }

        }

        public async Task<Vehicule> Obtenir(int id)
        {
            var reponse = await _httpClient.GetAsync(_apiUrl + id);
            if (reponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError("Vehicule avec ID {0} introuvable suite à une requête GET de API Favoris", id);
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API Favoris suite à une requête GET ");
            }
            return await _httpClient.GetFromJsonAsync<Vehicule>(_apiUrl+id);
        }

        public async Task<List<Vehicule>> ObtenirTout()
        {
            var reponse = await _httpClient.GetAsync(_apiUrl);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API Favoris suite à une requête GET ");
            }
            return await _httpClient.GetFromJsonAsync<List<Vehicule>>(_apiUrl);
        }
    }
}
