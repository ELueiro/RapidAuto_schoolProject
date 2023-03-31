using RapidAuto.Commandes.API.Model;
using System.Linq.Expressions;

namespace RapidAuto.Commandes.API.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}
