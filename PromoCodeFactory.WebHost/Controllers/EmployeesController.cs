using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Roles
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _roleRepository;
        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Get All Employees
        /// </summary> 
        /// <returns></returns>>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeeList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeeList;
        }
        /// <summary>
        /// Get Employee By Id
        /// </summary> 
        /// <returns></returns>>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description,
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount,
            };

            return employeeModel;
        }

        /// <summary>
        /// Create Employee
        /// </summary> 
        /// <returns></returns>>
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(CreateOrEditEmployeeRequest model)
        {
            Employee employee = new Employee()
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                AppliedPromocodesCount = model.AppliedPromocodesCount,
                Roles = await _roleRepository
                     .GetByCondition(x => model.RoleNames.Contains(x.Name)) as List<Role>
            };

            try
            {
                await _employeeRepository.CreateAsync(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Udpate Employee
        /// </summary> 
        /// <returns></returns>>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeAsync(Guid id, CreateOrEditEmployeeRequest model)
        {
            Employee employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return BadRequest();
            }
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.AppliedPromocodesCount = model.AppliedPromocodesCount;
            employee.Email = model.Email;
            employee.Roles = await _roleRepository.GetByCondition(x =>
                model.RoleNames.Contains(x.Name)) as List<Role>;
            try
            {
                await _employeeRepository.UpdateAsync(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }
        /// <summary>
        /// Delete Employee
        /// </summary> 
        /// <returns></returns>>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeAsync(Guid id)
        {
            Employee employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            try
            {
                await _employeeRepository.DeleteAsync(employee);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
    }
}
