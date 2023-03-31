using Newtonsoft.Json;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System.Text;


namespace RapidAuto.MVC.Services
{
    public class VehiculeMVCServiceProxy : IVehiculeMVCService
    {
        private readonly ILogger<VehiculeMVCServiceProxy> _logger;
        private readonly HttpClient _httpClient;
        private const string _apiUrl = "api/Vehicules/";

        public VehiculeMVCServiceProxy(HttpClient httpClient, ILogger<VehiculeMVCServiceProxy> logger)
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
                _logger.LogError("Vehicule avec ID {0} n'a pas pu être ajouté suite à une mauvaise requête POST de API vehicules", vehicule.Id);
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API vehicules suite à une requête POST ");
            }
            if (reponse.StatusCode == System.Net.HttpStatusCode.Created)
            {
                _logger.LogInformation(ActionsUtilisateursLogEvents.CreationVehicule, "Le vehicule avec ID {0} a été créé", vehicule.Id);
            }
        }

        public async Task Modifier(Vehicule vehicule)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(vehicule), Encoding.UTF8, "application/json");

            var reponse = await _httpClient.PutAsync(_apiUrl + vehicule.Id, content);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API vehicules suite à une requête PUT ");
            }
            if (reponse.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                _logger.LogInformation(ActionsUtilisateursLogEvents.ModificationVehicule, "Le vehicule avec ID {0} a été modifié", vehicule.Id);
            }

        }

        public async Task<Vehicule> Obtenir(int id)
        {
            var reponse = await _httpClient.GetAsync(_apiUrl + id);
            if (reponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError("Vehicule avec ID {0} introuvable suite à une requête GET de API vehicules",id);
            }
           
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API vehicules suite à une requête GET ");
            }
            if (reponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation(ActionsUtilisateursLogEvents.ConsultationVehicule,"Le vehicule avec ID {0} a été consulté", id);
            }
            return await _httpClient.GetFromJsonAsync<Vehicule>(_apiUrl + id);
        }

        public async Task<List<Vehicule>> ObtenirTout()
        {
            var reponse = await _httpClient.GetAsync(_apiUrl);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API vehicules suite à une requête GET ");
            }
            return await _httpClient.GetFromJsonAsync<List<Vehicule>>(_apiUrl);
        }

        public async Task Supprimer(int id)
        {
            var reponse = await _httpClient.DeleteAsync(_apiUrl + id);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API vehicules suite à une requête Delete ");
            }
            if (reponse.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                _logger.LogInformation(ActionsUtilisateursLogEvents.SuppressionVehicule, "Le vehicule avec ID {0} a été supprimer", id);
            }
        }
    }
}
