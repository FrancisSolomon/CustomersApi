using System.Collections.Generic;
using System.Threading.Tasks;

using CustomersApi.Configuration;
using CustomersApi.Models;
using CustomersApi.Services;
using CustomersApi.V1.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomersApi.V1.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("{id:long}", Name = RouteNames.GetCustomerById)]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var result = await _customerService.GetByIdAsync(id);

            return result == null ? NotFound() : (IActionResult)Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        public async Task<IActionResult> GetAsync([FromQuery]CustomerFilter filter)
        {
            var results = await _customerService.GetAsync(filter);

            return Ok(results);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Customer), 201)]
        public async Task<IActionResult> PostAsync([FromBody]DtoCustomer customer)
        {
            var result = await _customerService.PostAsync(customer);

            return CreatedAtRoute(RouteNames.GetCustomerById, new { id = result.CustomerId }, result);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _customerService.DeleteAsync(id);

            return NoContent();
        }
    }
}
