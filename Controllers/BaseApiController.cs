using Microsoft.AspNetCore.Mvc;
using NOLA_API.Application.Core;
using NOLA_API.Repositories;

namespace NOLA_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController<T>(IRepository<T> repository) : ControllerBase
    where T : class
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        var items = await repository.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<T>> GetById(object id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create(T item)
    {
        await repository.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = item }, item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] object id, [FromBody] T item)
    {
        var existingItem = await repository.GetByIdAsync(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        await repository.UpdateAsync(item);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(object id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        await repository.DeleteAsync(id);
        return NoContent();
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null)
            return Ok(result.Value);
        if (result.IsSuccess && result.Value == null) return NotFound();
        return BadRequest(result.Error);
    }
}