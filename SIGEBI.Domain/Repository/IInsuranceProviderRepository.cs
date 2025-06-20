using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Repository
{
    public interface IInsuranceProviderRepository
    {
        IEnumerable<InsuranceProvider> GetAll();
        InsuranceProvider GetById(int id);
        void Add(InsuranceProvider provider);
        void Update(InsuranceProvider provider);
        void Delete(int id);
    }
}
