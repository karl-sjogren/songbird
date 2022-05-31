using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Songbird.Web.Contracts;
using Songbird.Web.DataObjects;
using Songbird.Web.Models;

namespace Songbird.Web.Controllers;

[ApiController]
[Route("api/planning-board")]
public class PlanningBoardController : Controller {
    private readonly IPlanningBoardService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<PlanningBoardController> _logger;

    public PlanningBoardController(IPlanningBoardService service, IMapper mapper, ILogger<PlanningBoardController> logger) {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PlanningBoardDTO>> GetCurrentPlanningboardAsync(CancellationToken cancellationToken) {
        var board = await _service.GetCurrentPlanningBoardAsync(cancellationToken);
        if(board == null)
            return NotFound();

        return _mapper.Map<PlanningBoard, PlanningBoardDTO>(board);
    }

    [HttpGet("{startDate:datetime}")]
    public async Task<ActionResult<PlanningBoardDTO>> GetPlanningboardForDateAsync(DateTime startDate, CancellationToken cancellationToken) {
        var board = await _service.GetByDateAsync(startDate, cancellationToken);
        if(board == null)
            return NotFound();

        return _mapper.Map<PlanningBoard, PlanningBoardDTO>(board);
    }

    [HttpPost("{startDate:datetime}")]
    public async Task<ActionResult<PlanningBoardDTO>> GenerateFikaScheduleForDateAsync(DateTime startDate, CancellationToken cancellationToken) {
        var board = await _service.GeneratePlanningBoardDateAsync(startDate, cancellationToken);
        if(board == null)
            return NotFound();

        return _mapper.Map<PlanningBoard, PlanningBoardDTO>(board);
    }

    [HttpPut("{id:guid}/{userId:guid}/{projectId:guid}/{hours:double}")]
    public async Task<ActionResult<PlannedProjectTimeDTO>> SetUserProjectTimeAsync(Guid id, Guid userId, Guid projectId, double hours, CancellationToken cancellationToken) {
        var projectTime = await _service.SetUserProjectTimeAsync(id, userId, projectId, hours, cancellationToken);
        if(projectTime == null)
            return BadRequest();

        return _mapper.Map<PlannedProjectTime, PlannedProjectTimeDTO>(projectTime);
    }

    [HttpDelete("{id:guid}/{userId:guid}/{projectId:guid}")]
    public async Task<IActionResult> ClearUserProjectTimeAsync(Guid id, Guid userId, Guid projectId, CancellationToken cancellationToken) {
        await _service.ClearUserProjectTimeAsync(id, userId, projectId, cancellationToken);

        return NoContent();
    }

    [HttpGet("projects")]
    public async Task<ActionResult<ICollection<ProjectDTO>>> GetEligibleProjectsAsync(CancellationToken cancellationToken) {
        var projects = await _service.GetEligibleProjectsAsync(cancellationToken);

        return Ok(_mapper.Map<ICollection<Project>, ICollection<ProjectDTO>>(projects));
    }
}
