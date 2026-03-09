using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.DTOs;
using BizCover.Application.Interfaces;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.Queries.GetAllCars {
    public class GetAllCarsQueryHandler : IQueryHandler<GetAllCarsQuery, Result<List<CarDto>>> {

        private readonly ICarRepository _carRepository;
        private readonly ILogger<GetAllCarsQueryHandler> _logger;
        public GetAllCarsQueryHandler(ICarRepository carRepository, ILogger<GetAllCarsQueryHandler> logger) {
            this._carRepository = carRepository;
            this._logger = logger;
        }

        public async ValueTask<Result<List<CarDto>>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken) {
            _logger.LogInformation("Handling GetAllCarsQuery");
            var cars = await _carRepository.GetAllCarsAsync(cancellationToken);

            _logger.LogInformation("Successfully retrieved {Count} cars", cars?.Count() ?? 0);
            var mappedCars = cars?.Select(c => new CarDto {
                Id                  = c.Id,
                Make                = c.Make,
                Model               = c.Model,
                Year                = c.Year,
                Price               = c.Price,
                CountryManufactured = c.CountryManufactured,
                Colour              = c.Colour
            }).ToList() ?? new List<CarDto>();

            return Result<List<CarDto>>.Success(mappedCars);
        }
    }
}
