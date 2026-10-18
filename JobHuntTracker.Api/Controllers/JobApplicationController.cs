using JobHuntTracker.Api.Models;
using JobHuntTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobHuntTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController(IJobApplicationService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<JobApplication>> GetAll([FromQuery] ApplicationStatus? status) =>
        Ok(service.GetAll(status));

    [HttpGet("{id:guid}")]
    public ActionResult<JobApplication> GetById(Guid id)
    {
        var app = service.GetById(id);
        return app is null ? NotFound() : Ok(app);
    }

    [HttpPost]
    public ActionResult<JobApplication> Create(CreateJobApplicationDto dto)
    {
        var created = service.Add(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/status")]
    public ActionResult<JobApplication> UpdateStatus(Guid id, UpdateStatusDto dto)
    {
        var updated = service.UpdateStatus(id, dto.Status);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id) =>
        service.Delete(id) ? NoContent() : NotFound();
}