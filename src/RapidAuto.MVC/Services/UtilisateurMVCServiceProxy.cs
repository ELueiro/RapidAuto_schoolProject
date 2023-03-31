using Newtonsoft.Json;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System.Text;

namespace RapidAuto.MVC.Services
{
 
    public class UtilisateurMVCServiceProxy : IUtilisateurMVCService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UtilisateurMVCServiceProxy> _logger;
        private const string _apiUrl = "api/Utilisateurs/";

        public UtilisateurMVCServiceProxy(HttpClient httpClient, ILogger<UtilisateurMVCServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Utilisateur>> ObtenirTout()
        {
            var reponse = await _httpClient.GetAsync(_apiUrl);

            _ = LogReponseAPI(reponse.Content);

            return await reponse.Content.ReadFromJsonAsync<List<Utilisateur>>();

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API utilisateurs suite à une requête GET ");
            }
            return await _httpClient.GetFromJsonAsync<List<Utilisateur>>(_apiUrl);

        }
        

        public async Task  Ajouter(Utilisateur utilisateur)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(utilisateur), Encoding.UTF8, "application/json");

            var reponse = await _httpClient.PostAsync(_apiUrl, content);

            _ = LogReponseAPI(reponse.Content);

            if (reponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                _logger.LogError("Utilisateur avec ID {0} n'a pas pu être ajouté suite à une mauvaise requête POST de API utilisateurs", utilisateur.Id);
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API utilisateurs suite à une requête Post ");
            }

        }

        public async Task<Utilisateur> Obtenir(int id)
        {
            var reponse = await _httpClient.GetAsync(_apiUrl + id);
            if (reponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError("Utilisateur avec ID {0} introuvable suite à une requête GET de API utilisateurs", id);
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API utilisateurs suite à une requête GET ");
            }
            return await _httpClient.GetFromJsonAsync<Utilisateur>(_apiUrl + id);
        }

        public async Task Modifier(Utilisateur utilisateur)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(utilisateur), Encoding.UTF8, "application/json");


            var reponse=await _httpClient.PutAsync(_apiUrl + utilisateur.Id, content);
            _ = LogReponseAPI(reponse.Content);
            _ = await _httpClient.PutAsync(_apiUrl + utilisateur.Id, content);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API utilisateurs suite à une requête PUT ");
            }

        }

        public async Task  Supprimer(int id)
        {

            var reponse=await _httpClient.DeleteAsync(_apiUrl + id);
            _ = LogReponseAPI(reponse.Content);
        }

        private async Task LogReponseAPI(HttpContent httpContent)
        {
            var contenuReponse = await httpContent.ReadAsStringAsync();

            _logger.LogInformation($"Réponse de l'appel de l'API utilisateur : {contenuReponse}");

            var reponse = await _httpClient.DeleteAsync(_apiUrl);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API utilisateurs suite à une requête Delete ");
            }

        }
    }
}
