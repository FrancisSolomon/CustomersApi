using System.Collections.Generic;
using System.Threading.Tasks;

using CustomersApi.Models;
using CustomersApi.V1.Models;

namespace CustomersApi.Services
{
    public interface ICustomerService
    {
        Task<DtoCustomer> GetByIdAsync(long id);

        Task<List<DtoCustomer>> GetAsync(CustomerFilter filter);

        Task<DtoCustomer> PostAsync(DtoCustomer customer);

        Task DeleteAsync(long id);
    }
}
