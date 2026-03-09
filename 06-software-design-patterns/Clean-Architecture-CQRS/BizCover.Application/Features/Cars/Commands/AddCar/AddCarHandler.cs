using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.DTOs;
using BizCover.Application.Features.Cars.Mappers;
using BizCover.Application.Interfaces;
using BizCover.Domain.Entities;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BizCover.Application.Features.Cars.Commands.AddCar {
    public class AddCarHandler : IRequestHandler<AddCarCommand, Result<CarDto>> {
        private readonly ICarRepository _carRepository;
        private readonly ILogger<AddCarHandler> _logger;
        public AddCarHandler(ICarRepository carRepository, ILogger<AddCarHandler> logger) {
            _carRepository = carRepository;
            _logger = logger;
        }

        public async ValueTask<Result<CarDto>> Handle(AddCarCommand request, CancellationToken cancellationToken) {
            _logger.LogInformation("Handling AddCarCommand for car: {@Car}" , JsonSerializer.Serialize(request.car));
            Car car = CarDtoMapper.MapToDomainCar(request.car);

            await _carRepository.AddAsync(car);
            _logger.LogInformation(JsonSerializer.Serialize("Successfully added car: {@Car}"), car);
            return Result<CarDto>.Success(CarDtoMapper.MapToDTO(car));
        }
    }
}
