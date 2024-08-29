using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.DataAccess.Data;
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
        private readonly IRepository<EmployeeRole> _employeeRoleRepository;
        private readonly PromoCodeFactoryDataContext _dbContext;
        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> roleRepository, IRepository<EmployeeRole> employeeRoleRepository, PromoCodeFactoryDataContext dbContext)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _employeeRoleRepository = employeeRoleRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get All Employees
        /// </summary> 
        /// <returns></returns>>
        [HttpGet]
        public async Task<ActionResult<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeeList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return Ok(employeeList);
        }

        /// <summary>
        /// Get Employee By Id
        /// </summary> 
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _dbContext.Employees
                                            .Include(e => e.EmployeeRoles)
                                            .ThenInclude(er => er.Role)
                                            .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                Roles = employee.EmployeeRoles
                    .Select(er => new RoleItemResponse()
                    {
                        Id = er.Role.Id,
                        Name = er.Role.Name,
                        Description = er.Role.Description,
                    })
                    .ToList(),
                AppliedPromocodesCount = employee.AppliedPromocodesCount,
            };

            return Ok(employeeModel);
        }

        /// <summary>
        /// Create Employee
        /// </summary> 
        /// <returns></returns>>
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(CreateOrEditEmployeeRequest model)
        {
            var roles = (await _roleRepository.GetByCondition(x => model.RoleNames.Contains(x.Name))).ToList();

            if (roles == null || !roles.Any())
            {
                return BadRequest("No roles found for the provided role names.");
            }

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                AppliedPromocodesCount = model.AppliedPromocodesCount
            };

            try
            {
                await _employeeRepository.CreateAsync(employee);
                await _employeeRepository.SaveChangesAsync(); 

                var employeeRoles = roles.Select(role => new EmployeeRole
                {
                    EmployeeId = employee.Id,
                    RoleId = role.Id
                }).ToList();

                var validRoleIds = roles.Select(r => r.Id).ToHashSet();
                var invalidRoles = employeeRoles.Where(er => !validRoleIds.Contains(er.RoleId)).ToList();
                if (invalidRoles.Any())
                {
                    return BadRequest("Some roles are invalid or do not exist.");
                }

                foreach (var employeeRole in employeeRoles)
                {
                    _dbContext.EmployeeRoles.Add(employeeRole);
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
        /// Update Employee
        /// </summary> 
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeAsync(Guid id, CreateOrEditEmployeeRequest model)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var roles = (await _roleRepository.GetByCondition(x => model.RoleNames.Contains(x.Name))).ToList();

            if (roles == null || !roles.Any())
            {
                return BadRequest("No roles found for the provided role names.");
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.AppliedPromocodesCount = model.AppliedPromocodesCount;

            var existingRoles = (await _employeeRoleRepository.GetByCondition(er => er.EmployeeId == id)).ToList();

            foreach (var existingRole in existingRoles)
            {
                await _employeeRoleRepository.DeleteAsync(existingRole);
            }

            var newEmployeeRoles = roles.Select(role => new EmployeeRole
            {
                EmployeeId = id,
                RoleId = role.Id
            }).ToList();

            foreach (var employeeRole in newEmployeeRoles)
            {
                await _employeeRoleRepository.CreateAsync(employeeRole);
            }

            try
            {
                await _employeeRepository.UpdateAsync(employee);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete Employee
        /// </summary> 
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeRoles = await _employeeRoleRepository.GetByCondition(er => er.EmployeeId == id);
            foreach (var employeeRole in employeeRoles)
            {
                await _employeeRoleRepository.DeleteAsync(employeeRole);
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
