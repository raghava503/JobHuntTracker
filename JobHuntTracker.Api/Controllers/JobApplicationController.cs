using JobHuntTracker.Api.Models;
using JobHuntTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController(IJobApplicationService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobApplication>>> GetAll([FromQuery] ApplicationStatus? status) =>
        Ok(await service.GetAllAsync(status));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobApplication>> GetById(Guid id)
    {
        var app = await service.GetByIdAsync(id);
        return app is null ? NotFound() : Ok(app);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplication>> Create(CreateJobApplicationDto dto)
    {
        var created = await service.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<JobApplication>> UpdateStatus(Guid id, UpdateStatusDto dto)
    {
        var updated = await service.UpdateStatusAsync(id, dto.Status);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}