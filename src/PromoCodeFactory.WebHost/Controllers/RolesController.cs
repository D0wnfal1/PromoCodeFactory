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
    public class RolesController : ControllerBase
    {
        private readonly IRepository<Role> _roleRepository;
        public RolesController(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }
        /// <summary>
        /// Get All Roles
        /// </summary>
        [HttpGet]
        public async Task<List<RoleItemResponse>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            var roleList = roles.Select(x => new RoleItemResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToList();

            return roleList;
        }
        /// <summary>
        /// Get Role By Id
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoleItemResponse>> GetRoleByIdAsync(Guid id) 
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null) 
            {
                return NotFound();
            }

            var roleModel = new RoleItemResponse()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
            };
            return roleModel;
        }
    }
}
