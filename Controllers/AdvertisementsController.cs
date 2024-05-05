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
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertisement(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetOne.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> AddAdvertisement(Advertisement advertisement)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Advertisement = advertisement }));
        }

        //[Authorize(Policy = "IsOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAdvertisement(Guid id, Advertisement advertisement)
        {
            advertisement.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Advertisement = advertisement }));
        }

        //[Authorize(Policy = "IsOwner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvertisement(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command {  Id = id }));
        }

        // [HttpPost("{id}/visit")]
        // public async Task<IActionResult> Visit(Guid id, [FromHeader]Guid advertisementVisitor)
        // {
        //     return HandleResult(await Mediator.Send(new Visit.Command { Id = id, Visitor = visitorId }));
        // }
    }
}