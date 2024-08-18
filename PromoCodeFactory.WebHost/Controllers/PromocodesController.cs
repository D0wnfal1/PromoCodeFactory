using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// Get All Promocode
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            //TODO: get all Promocode 
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a promotional code and issue it to customers with the specified preference
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Create a promotional code and issue it to customers with the specified preference
            throw new NotImplementedException();
        }
    }
}
