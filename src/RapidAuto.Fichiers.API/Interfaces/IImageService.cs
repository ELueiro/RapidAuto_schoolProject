using Microsoft.AspNetCore.Http;
using RapidAuto.Fichiers.API.Models;

namespace RapidAuto.Fichiers.API.Interfaces
{
    public interface IImageService
    {
        public Task<List<string>> SauvegarderImages(Fichier images);
        
    }
}
