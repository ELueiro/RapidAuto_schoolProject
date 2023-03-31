using Newtonsoft.Json;
using RapidAuto.MVC.Interface;
using RapidAuto.MVC.Models;
using System.Text;

namespace RapidAuto.MVC.Services
{
    public class CommanderMVCServiceProxy : ICommandeMVCService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CommanderMVCServiceProxy> _logger;
        private const string _apiUrl = "api/commandes/";
        public CommanderMVCServiceProxy(HttpClient httpClient, ILogger<CommanderMVCServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger= logger;
        }

        public async Task<List<Commande>> ObtenirTout()
        {
            var reponse = await _httpClient.GetAsync(_apiUrl);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API commandes suite à une requête GET ");
            }
            return await  _httpClient.GetFromJsonAsync<List<Commande>>(_apiUrl);
        }


        public async Task<HttpResponseMessage> Ajouter(InformationDemandeCommande informationDemande)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(informationDemande), Encoding.UTF8, "application/json");

            var reponse = await _httpClient.PostAsync(_apiUrl, content);
            // _ = LogReponseAPI(reponse.Content);
            if (reponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                _logger.LogError("Utilisateur avec ID {0}  n'a pas pu commander la voiture avec ID {1} suite à une mauvaise requête POST de API commandes", informationDemande.IdUtilisateur, informationDemande.IdVoiture);
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API commandes suite à une requête POST ");
            }
            if (reponse.StatusCode == System.Net.HttpStatusCode.Created)
            {
                _logger.LogInformation(ActionsUtilisateursLogEvents.EnregistrementCommande, "Le vehicule avec ID {0} a été commandé par l'utilisateur avec ID {1}", informationDemande.IdUtilisateur, informationDemande.IdVoiture);
            }
            return reponse;

        }

        
        public async  Task<Commande> Obtenir(int id)
        {
            var reponse = await _httpClient.GetAsync(_apiUrl + id);
            if (reponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError("Commandes avec ID {0} introuvable suite à une requête GET de API commandes", id);
            }

            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API commandes suite à une requête GET ");
            }
            return  await _httpClient.GetFromJsonAsync<Commande>(_apiUrl + id);
        }


        public async Task Supprimer(int id)
        {
            var reponse = await _httpClient.DeleteAsync(_apiUrl + id);
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API commandes suite à une requête Delete ");
            }
        }

        public async Task Modifier(Commande commande)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(commande), Encoding.UTF8, "application/json");

            var reponse = await _httpClient.PutAsync(_apiUrl + commande.Id, content);
           
            if (reponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.LogCritical("Pas de reponse du serveur de API commandes suite à une requête PUT ");
            }
        }
    }
}

    

