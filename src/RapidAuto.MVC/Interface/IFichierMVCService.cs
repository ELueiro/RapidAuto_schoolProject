using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interface
{
    public interface IFichierMVCService
    {
        public Task<HttpResponseMessage> Ajouter(Fichier fichier);
    }
}
