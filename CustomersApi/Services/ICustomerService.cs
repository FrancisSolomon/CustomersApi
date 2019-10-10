using System.Collections.Generic;
using System.Threading.Tasks;

using CustomersApi.Models;
using CustomersApi.V1.Models;

namespace CustomersApi.Services
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetByIdAsync(long id);

        Task<List<CustomerDto>> GetAsync(CustomerFilter filter);

        Task<CustomerDto> PostAsync(CustomerDto customer);

        Task DeleteAsync(long id);
    }
}
