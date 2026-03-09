using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.DTOs;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.Commands.UpdateCar {
    public record UpdateCarCommand(CarDto car) : IRequest<Result<CarDto>>;
}
