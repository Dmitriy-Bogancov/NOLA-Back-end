using Microsoft.AspNetCore.Mvc;
using NOLA_API.Application.Drafts;
using NOLA_API.DataModels;

namespace NOLA_API.Controllers
{
   // [Authorize(Policy = "IsOwner")]
    public class DraftsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetDrafts()
        {
            return HandleResult(await Mediator.Send(new Show.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDraft(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetOne.Query { Id = id }));
        }

        [HttpPost]        
        public async Task<IActionResult> AddDraft(Draft draft)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Draft = draft }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDraft(Guid id, Draft draft)
        {
            draft.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Draft = draft }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDraft(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id, Draft draft)
        {
            return HandleResult(await Mediator.Send(new PublishDraft.Command { Id = id, Draft = draft }));
        }
    }
}