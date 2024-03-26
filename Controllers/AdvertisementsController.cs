using Microsoft.AspNetCore.Mvc;
using NOLA_API.Application.Advertisements;

namespace NOLA_API.Controllers
{
    public class AdvertisementsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAdvertisements()
        {
            return HandleResult(await Mediator.Send(new Show.Query()));
        }
    }
}