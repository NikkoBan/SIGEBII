using SIGEBI.Persistence.Interface;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly List<T> _data;

        public BaseRepository()
        {
            _data = new List<T>();
        }

        public T? GetById(int id)
        {
            // Esta es una simplificación para la simulación en memoria.
            // En EF, sería context.Set<T>().Find(id);
            var property = typeof(T).GetProperty(typeof(T).Name + "Id");
            if (property == null)
            {
                // Fallback para propiedades con nombre 'Id' directamente
                property = typeof(T).GetProperty("Id");
            }

            if (property == null)
            {
                throw new InvalidOperationException($"La entidad {typeof(T).Name} no tiene una propiedad Id definida.");
            }

            return _data.FirstOrDefault(item => (int)property.GetValue(item)! == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _data;
        }

        public void Add(T entity)
        {
            // En un escenario real, la DB asignaría el ID. Aquí lo simulamos.
            var property = typeof(T).GetProperty(typeof(T).Name + "Id");
            if (property == null)
            {
                property = typeof(T).GetProperty("Id");
            }
            if (property != null && property.PropertyType == typeof(int))
            {
                int nextId = _data.Any() ? _data.Max(item => (int)property.GetValue(item)!) + 1 : 1;
                property.SetValue(entity, nextId);
            }
            _data.Add(entity);
        }

        public void Update(T entity)
        {
            var property = typeof(T).GetProperty(typeof(T).Name + "Id");
            if (property == null)
            {
                property = typeof(T).GetProperty("Id");
            }
            if (property == null) return; // No se puede actualizar sin una clave

            int entityId = (int)property.GetValue(entity)!;
            var existingEntity = GetById(entityId);
            if (existingEntity != null)
            {
                _data[_data.IndexOf(existingEntity)] = entity; // Simple reemplazo
            }
        }

        public void Delete(int id)
        {
            var entityToDelete = GetById(id);
            if (entityToDelete != null)
            {
                _data.Remove(entityToDelete);
            }
        }
    }
}
