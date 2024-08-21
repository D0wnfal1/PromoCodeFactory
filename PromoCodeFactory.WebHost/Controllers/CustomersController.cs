using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
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
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<PromoCode> _promocodeRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;
        private readonly PromoCodeFactoryDataContext _dbContext;
        public CustomersController(IRepository<Customer> customerRepository, PromoCodeFactoryDataContext dbContext, IRepository<Preference> preferenceRepository, IRepository<PromoCode> promocodeRepository, IRepository<CustomerPreference> customerPreferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _promocodeRepository = promocodeRepository;
            _customerPreferenceRepository = customerPreferenceRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <returns>List of Customers</returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var customerList = customers.Select(x =>
            new CustomerShortResponse()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
            }
            ).ToList();
            return Ok(customerList);
        }

        /// <summary>
        /// Get Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Customer</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _dbContext.Customers
                                            .Include(c => c.CustomerPreferences)
                                            .ThenInclude(cp => cp.Preference)
                                            .Include(c => c.PromoCodes)
                                            .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerModel = new CustomerResponse()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Preferences = customer.CustomerPreferences
                                        .Select(cp => new PreferenceResponse()
                                        {
                                            Id = cp.Preference.Id,
                                            Name = cp.Preference.Name,
                                        })
                                        .ToList(),
                PromoCodes = customer.PromoCodes
                                      .Select(cp => new PromoCodeShortResponse()
                                      {
                                          Id = cp.Id,
                                          Code = cp.Code,
                                          ServiceInfo = cp.ServiceName,
                                          BeginDate = cp.BeginDate,
                                          EndDate = cp.EndDate,
                                          PartnerName = cp.PartnerName
                                      }).ToList(),
            };

            return Ok(customerModel);
        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response Code</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = (await _preferenceRepository.GetByCondition(x => request.PreferenceIds.Contains(x.Id))).ToList();
            if (!preferences.Any())
            {
                return BadRequest("No Preferences found for the provided preferences id.");
            }
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                
            };
            try
            {
                await _customerRepository.CreateAsync(customer);
                await _customerRepository.SaveChangesAsync();

                var customerPreferences = preferences.Select(preference => new CustomerPreference
                {
                    CustomerId = customer.Id,
                    PreferenceId = preference.Id,
                }).ToList();

                foreach (var customerPreference in customerPreferences)
                {
                    _dbContext.CustomerPreferences.Add(customerPreference);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns>Response Code</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var preferences = (await _preferenceRepository.GetByCondition(x => request.PreferenceIds.Contains(x.Id))).ToList();
            if (preferences == null || !preferences.Any())
            {
                return BadRequest("No preference found for the provided preferences id.");
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;

            var existingPreferences = (await _customerPreferenceRepository.GetByCondition(cp => cp.PreferenceId == id)).ToList();

            foreach (var existingPreference in existingPreferences)
            {
                await _customerPreferenceRepository.DeleteAsync(existingPreference);
            }

            var newCustomerPreferences = preferences.Select(preference => new CustomerPreference
            {
                CustomerId = id,
                PreferenceId = preference.Id,
            }).ToList();

            foreach (var customerPreference in newCustomerPreferences)
            {
                await _customerPreferenceRepository.CreateAsync(customerPreference);
            }

            try
            {
                await _customerRepository.UpdateAsync(customer);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Response Code</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }


            var customerPromocodes = await _promocodeRepository.GetByCondition(cp => cp.CustomerId == id);
            foreach (var customerPromocode in customerPromocodes)
            {
                await _promocodeRepository.DeleteAsync(customerPromocode);
            }
            try
            {
                await _customerRepository.DeleteAsync(customer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
    }
}
