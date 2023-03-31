using Microsoft.EntityFrameworkCore;
using RapidAuto.Vehicules.API.Interfaces;
using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly VehiculeContext _dbContext;

        public Repository(VehiculeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Edit(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public virtual IEnumerable<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                .Where(predicate)
                .AsEnumerable();
        }




    }
}
