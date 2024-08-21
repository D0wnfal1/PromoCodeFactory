using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Promocodes
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController : ControllerBase
    {
        private readonly IRepository<PromoCode> _promocodeRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly PromoCodeFactoryDataContext _dbContext;

        public PromocodesController(IRepository<PromoCode> promocodeRepository, IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository, IRepository<CustomerPreference> customerPreferenceRepository, IRepository<Employee> employeeRepository, PromoCodeFactoryDataContext dbContext)
        {
            _promocodeRepository = promocodeRepository;
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _customerPreferenceRepository = customerPreferenceRepository;
            _employeeRepository = employeeRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get All Promocodes
        /// </summary>
        /// <returns>List of Promocodes</returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodeList = await _promocodeRepository.GetAllAsync();
            return Ok(promocodeList);
        }

        /// <summary>
        /// Create a promotional code and issue it to customers with the specified preference
        /// </summary>
        /// <returns>Response Code</returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var preference = await _dbContext.Preferences
                                             .FirstOrDefaultAsync(p => p.Name == request.Preference);

            if (preference == null)
            {
                return NotFound("Preference not found");
            }

            var customerPreferences = await _dbContext.CustomerPreferences
                .Include(cp => cp.Customer)
                .Where(cp => cp.PreferenceId == preference.Id)
                .ToListAsync();

            if (!customerPreferences.Any())
            {
                return BadRequest("No customers found with the specified preferences.");
            }

            var customers = customerPreferences
                .Select(cp => cp.Customer)
                .Where(c => c != null)
                .Distinct()
                .ToList();

            if (!customers.Any())
            {
                return BadRequest("No valid customers found with the specified preferences.");
            }

            var employee = await _dbContext.Employees
                                           .FirstOrDefaultAsync(e => e.Id.ToString() == request.PartnerName);
            if (employee == null)
            {
                return BadRequest("No valid employee found with the specified Id.");
            }

            foreach (var customer in customers)
            {
                var promoCode = new PromoCode
                {
                    Code = request.PromoCode,
                    ServiceName = request.ServiceInfo,
                    PartnerName = request.PartnerName,
                    BeginDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    CustomerId = customer.Id,
                    PreferenceId = preference.Id,
                    PartnerManagerId = employee.Id,
                };

                await _promocodeRepository.CreateAsync(promoCode);

                customer.PromoCodes.Add(promoCode);
                await _customerRepository.UpdateAsync(customer);
                employee.AppliedPromocodesCount += 1;
                await _employeeRepository.UpdateAsync(employee);
            }

            return NoContent();
        }

    }
}
