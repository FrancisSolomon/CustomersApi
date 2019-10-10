using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using CustomersApi.DataAccess;
using CustomersApi.Models;
using CustomersApi.V1.Models;

using FluentValidation;

using LinqKit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomersApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDbContext _context;

        private readonly IMapper _mapper;

        private readonly ILogger<CustomerService> _logger;

        private readonly IValidator<CustomerDto> _customerValidator;

        public CustomerService(CustomerDbContext context, IMapper mapper, ILogger<CustomerService> logger, IValidator<CustomerDto> customerValidator)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _customerValidator = customerValidator;
        }

        public async Task<CustomerDto> GetByIdAsync(long id)
        {
            var result = await _context.Customers.FindAsync(id);

            return _mapper.Map<CustomerDto>(result);
        }

        public async Task<List<CustomerDto>> GetAsync(CustomerFilter filter)
        {
            var predicate = PredicateBuilder.New<Customer>(true);

            if (filter.IncludeInactive.GetValueOrDefault(false) == false)
            {
                predicate = predicate.And(c => c.IsActive);
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                predicate = predicate.And(
                    c => c.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase));
            }

            if (filter.MinimumSpend.HasValue)
            {
                predicate = predicate.And(c => c.TotalOrderValue >= filter.MinimumSpend.Value);
            }

            var query = _context.Customers.Where(predicate);

            if (filter.Include != null && filter.Include.Any())
            {
                if (filter.Include.Contains("contacts", StringComparer.InvariantCultureIgnoreCase))
                {
                    query = query.Include(c => c.Contacts);
                }

                if (filter.Include.Contains("addresses", StringComparer.InvariantCultureIgnoreCase))
                {
                    query = query.Include(c => c.Contacts).ThenInclude(c => c.Addresses);
                }
            }

            var results = await query.ToListAsync();

            return _mapper.Map<List<CustomerDto>>(results);
        }

        public async Task<CustomerDto> PostAsync(CustomerDto customerDto)
        {
            await _customerValidator.ValidateAndThrowAsync(customerDto);
            var customer = _mapper.Map<Customer>(customerDto);
            var updatedCustomer = await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(updatedCustomer.Entity);
        }

        public async Task DeleteAsync(long id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);

            if (customer != default)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
