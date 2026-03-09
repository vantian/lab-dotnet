using BizCover.Application.Features.Cars.Commands.AddCar;
using BizCover.Application.Features.Cars.Commands.UpdateCar;
using BizCover.Application.Features.Cars.DTOs;
using BizCover.Application.Features.Cars.Queries.GetAllCars;
using BizCover.Application.Features.Cars.Queries.GetCarsDiscounts;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BizCover.Api.Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase{

    private readonly IMediator _mediator;
    public CarsController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet]
    /// returns a list of all cars
    public async Task<ActionResult<IList<CarDto>>> GetAllAsync(CancellationToken cancellationToken) {
        var result = await _mediator.Send(new GetAllCarsQuery(), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("discount")]
    /// returns a list of all cars with their discounts
    public async Task<ActionResult<CarDiscountDto>> GetDiscountsAsync([FromQuery] List<int> ids, CancellationToken cancellationToken) {
        var result = await _mediator.Send(new CalculateCarsDiscountsQuery(ids), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    /// add new car
    public async Task<ActionResult<CarDto>> AddCarAsync(CarDto input, CancellationToken cancellationToken) {
        var result = await _mediator.Send(new AddCarCommand(input), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPut]
    /// update existing car
    public async Task<ActionResult<CarDto>> UpdateCarAsync(CarDto input, CancellationToken cancellationToken) {
        var result = await _mediator.Send(new UpdateCarCommand(input), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
