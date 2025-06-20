using SIGEBI.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Domain.Repository
{
    public class NetworkTypeRepository
    {
        private readonly List<NetworkType> _networkTypes = new();

        public IEnumerable<NetworkType> GetAll() => _networkTypes;

        public NetworkType GetById(int id) => _networkTypes.FirstOrDefault(n => n.Id == id);

        public void Add(NetworkType networkType) => _networkTypes.Add(networkType);

        public void Update(NetworkType networkType)
        {
            var existing = GetById(networkType.Id);
            if (existing != null)
            {
                existing.Name = networkType.Name;
                // Update other properties as needed
            }
        }

        public void Delete(int id)
        {
            var networkType = GetById(id);
            if (networkType != null)
                _networkTypes.Remove(networkType);
        }
    }
}
