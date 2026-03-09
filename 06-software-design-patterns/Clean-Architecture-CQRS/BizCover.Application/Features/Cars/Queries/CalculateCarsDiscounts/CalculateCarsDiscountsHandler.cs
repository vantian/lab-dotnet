using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.DTOs;
using BizCover.Application.Interfaces;
using BizCover.Domain.Entities;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.Queries.GetCarsDiscounts {
    public class CalculateCarsDiscountsHandler : IRequestHandler<CalculateCarsDiscountsQuery, Result<CarDiscountDto>> {

        private readonly ICarRepository _carRepository;
        private readonly ILogger<CalculateCarsDiscountsHandler> _logger;
        public CalculateCarsDiscountsHandler(ICarRepository carRepository, ILogger<CalculateCarsDiscountsHandler> logger) {
            _carRepository = carRepository;
            _logger = logger;
        }
        public async ValueTask<Result<CarDiscountDto>> Handle(CalculateCarsDiscountsQuery request, CancellationToken cancellationToken) {

            _logger.LogInformation("Calculating discounts for cars with IDs: {CarIds}", string.Join(", ", request.carIds));

            // get all cars
            var allCars = await _carRepository.GetAllCarsAsync(cancellationToken);

            // filter cars by request carIds
            var filteredCars = allCars.Where(car => request.carIds.Contains(car.Id)).ToList();

            var responseDto = new CarDiscountDto(filteredCars);

            CalculateDiscount(responseDto);

            return Result<CarDiscountDto>.Success(responseDto);
        }

        private void CalculateDiscount(CarDiscountDto carDiscountDto) {

            if (carDiscountDto.Cars.Count > 2) {
                // 3% discount for more than 2 cars
                carDiscountDto.TotalDiscount += carDiscountDto.TotalPrice * 0.03m;
            }

            if (carDiscountDto.TotalPrice > 100_000m) {
                // 5% discount for total price over 100,000
                carDiscountDto.TotalDiscount += carDiscountDto.TotalPrice * 0.05m;
            }

            if (carDiscountDto.TotalOldCars > 0) {
                // 10% for each old car
                decimal totalDiscountForOldCars = 0;
                foreach (var car in carDiscountDto.Cars) {
                    if (car.Year < 2000) {
                        totalDiscountForOldCars += (car.Price ?? 0) * 0.10m;
                    }
                }
                carDiscountDto.TotalDiscount += totalDiscountForOldCars;
            }
        }
    }
}
