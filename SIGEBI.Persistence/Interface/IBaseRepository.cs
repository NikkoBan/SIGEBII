
namespace SIGEBI.Persistence.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
