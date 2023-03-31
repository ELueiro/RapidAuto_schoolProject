namespace RapidAuto.Favoris.API.Interface
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> List();
        void Add(T entity);
    }
}
