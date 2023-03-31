using Microsoft.Extensions.Caching.Memory;
using RapidAuto.Favoris.API.Interface;
using RapidAuto.Favoris.API.Models;

namespace RapidAuto.Favoris.API
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IMemoryCache _memoryCache;
        protected readonly string _cacheKey = "vehiculesFavoris";

        public Repository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add(T entity)
        {
            //_memoryCache.TryGetValue(_cacheKey, out List<T>? items);
            //items.Add(entity);
            ////Configure la durée de mise en cache
            //var cacheEntryOptions = new MemoryCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(86400),
            //    Size = items.Count * 10
            //};
            ////ajoute l'information dans le cache
            //_memoryCache.Set(_cacheKey, items, cacheEntryOptions);
            bool x = _memoryCache.TryGetValue(_cacheKey, out List<T>? items);


            //Verifie si l'information existe dans le cache
            if (!x || items == null)
            {
                items = new List<T>();
                //Configure la durée de mise en cache
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                    Size = items.Count * 10
                };
                //ajoute l'information dans le cache
                items.Add(entity);
                _memoryCache.Set(_cacheKey, items, cacheEntryOptions);
            }
            items.Add(entity);  
        }

        public virtual T GetById(int id)
        {
            _memoryCache.TryGetValue(_cacheKey, out List<T>? items);
            return items.FirstOrDefault(x => x.Id == id);
            
        }


        public virtual IEnumerable<T> List()
        {
            
            bool x = _memoryCache.TryGetValue(_cacheKey, out List<T>? items);
            
           

            //Verifie si l'information existe dans le cache
            if (!x || items == null)
            {
                items = new List<T>();
                //Configure la durée de mise en cache
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {                 
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(86400),
                    Size = items.Count * 10
                };
                //ajoute l'information dans le cache
                _memoryCache.Set(_cacheKey, items, cacheEntryOptions);
            }

            if (items != null)
            return items.AsEnumerable();
            else return Enumerable.Empty<T>();
            
        }

        
    }
}
