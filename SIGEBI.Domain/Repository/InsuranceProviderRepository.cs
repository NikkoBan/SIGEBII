using SIGEBI.Domain.Entities;

namespace SIGEBI.Domain.Repository
{
    public class InsuranceProviderRepository
    {
        private readonly List<InsuranceProvider> _providers = new();

        // Example: Relate InsuranceProvider with Users (if needed)
        private readonly List<User> _users = new();

        public IEnumerable<InsuranceProvider> GetAll() => _providers;

        public InsuranceProvider GetById(int id) => _providers.FirstOrDefault(p => p.Id == id);

        public void Add(InsuranceProvider provider) => _providers.Add(provider);

        public void Update(InsuranceProvider provider)
        {
            var existing = GetById(provider.Id);
            if (existing != null)
            {
                existing.Name = provider.Name;
                // Update other properties as needed
            }
        }

        public void Delete(int id)
        {
            var provider = GetById(id);
            if (provider != null)
                _providers.Remove(provider);
        }

        // Example: Get all users for a provider (if you have such a relationship)
        public IEnumerable<User> GetUsersByProviderId(int providerId)
        {
            // Implement logic if User has a ProviderId property
            return _users.Where(u => u.UserId == providerId);
        }
    }
}
