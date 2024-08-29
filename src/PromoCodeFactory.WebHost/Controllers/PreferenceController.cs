using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.WebHost.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PreferenceController : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly PromoCodeFactoryDataContext _dbContext;
        public PreferenceController(IRepository<Preference> preferenceRepository, PromoCodeFactoryDataContext dbContext)
        {
            _preferenceRepository = preferenceRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get List of Preferences
        /// </summary>
        /// <returns>List of Preferences</returns>
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var prefernceList = await _preferenceRepository.GetAllAsync();
            return Ok(prefernceList);
        }
    }
}
