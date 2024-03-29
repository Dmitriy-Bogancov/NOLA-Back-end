using Microsoft.AspNetCore.Mvc;
using NOLA_API.Application.Advertisements;
using NOLA_API.DataModels;

namespace NOLA_API.Controllers
{
    public class AdvertisementsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAdvertisements()
        {
            return HandleResult(await Mediator.Send(new Show.Query()));
        }

        //xpath to get advertisement by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvertisement(int id)
        {
            return HandleResult(await Mediator.Send(new Show.Query {  }));
        }
    }
}