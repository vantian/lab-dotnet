using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.DTOs;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.Commands.AddCar {
    public record AddCarCommand(CarDto car) : IRequest<Result<CarDto>>;
}
