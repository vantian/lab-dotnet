using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.Commands.AddCar;
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

namespace BizCover.Application.Features.Cars.Commands.UpdateCar {
    public class UpdateCarHandler : IRequestHandler<UpdateCarCommand, Result<CarDto>> {

        private readonly ICarRepository _carRepository;
        private readonly ILogger<UpdateCarHandler> _logger;
        public UpdateCarHandler(ICarRepository carRepository, ILogger<UpdateCarHandler> logger) {
            _carRepository = carRepository;
            _logger = logger;
        }

        public async ValueTask<Result<CarDto>> Handle(UpdateCarCommand request, CancellationToken cancellationToken) {
            
            _logger.LogInformation("Handling UpdateCarCommand for car: {@Car}" , JsonSerializer.Serialize(request.car));

            // check if ID is exists
            var allCars = await _carRepository.GetAllCarsAsync();
            if (!allCars.Any(c => c.Id == request.car?.Id)) {
                return Result<CarDto>.Failure($"Car with ID {request.car?.Id} does not exist.");
            }

            Car car = CarDtoMapper.MapToDomainCar(request.car);

            await _carRepository.UpdateAsync(car);
            _logger.LogInformation("Successfully updated car: {@Car}", JsonSerializer.Serialize(car));
            return Result<CarDto>.Success(CarDtoMapper.MapToDTO(car));
        }
    }
}
