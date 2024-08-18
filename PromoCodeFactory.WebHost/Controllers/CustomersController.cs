using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Clients
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            //TODO: Add get List of Customers
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Add client receipt along with promotional codes issued to him
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Add creation of a new client along with its preferences
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Update customer details along with their preferences
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Removing a client along with the promotional codes issued to him
            throw new NotImplementedException();
        }
    }
}
