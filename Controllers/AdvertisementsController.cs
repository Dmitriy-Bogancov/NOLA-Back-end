using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOLA_API.Application.Advertisements;
using NOLA_API.DataModels;

namespace NOLA_API.Controllers
{
    public class AdvertisementsController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertisements()
        {
            return HandleResult(await Mediator.Send(new Show.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvertisement(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetOne.Query { Id = id }));
        }
    }
}