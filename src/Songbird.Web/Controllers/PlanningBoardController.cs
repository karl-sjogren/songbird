using System;
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
    public async Task<ActionResult<PlanningBoardDTO>> GenerateFikaScheduleForDateAsync(DateTime startDate, CancellationToken cancellationToken) {
        var board = await _service.GeneratePlanningBoardDateAsync(startDate, cancellationToken);
        if(board == null)
            return NotFound();

        return _mapper.Map<PlanningBoard, PlanningBoardDTO>(board);
    }
}
