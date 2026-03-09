using BizCover.Application.Features.Cars.Commands.AddCar;
using BizCover.Application.Features.Cars.Commands.UpdateCar;
using BizCover.Application.Features.Cars.Queries.GetCarsDiscounts;
using BizCover.Application.Interfaces;
using BizCover.Domain.Entities;
using Mediator;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BizCover.Api.Cars.Tests {
    public class CarsDiscountTest {
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CalculateCarsDiscountsHandler> _logger;
        private readonly CalculateCarsDiscountsHandler _handler;
        public CarsDiscountTest() {
            _carRepository = Substitute.For<ICarRepository>();
            _logger = Substitute.For<ILogger<CalculateCarsDiscountsHandler>>();
            _handler = new CalculateCarsDiscountsHandler(_carRepository, _logger);
        }

        [Fact]
        public async Task Should_Filter_Cars_By_Request_CarIds() {
            // Arrange
            var cars = new List<Car> {
                new() { Id = 1, Year = 2010, Price = 50000 },
                new() { Id = 2, Year = 2015, Price = 30000 },
                new() { Id = 3, Year = 2020, Price = 20000 }
            };

            _carRepository.GetAllCarsAsync(Arg.Any<CancellationToken>())
                .Returns(cars);

            // mocked only two cars in repo
            var query = new CalculateCarsDiscountsQuery (
                new List<int> { 1, 3 }
            );

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Cars.Count);
            Assert.Contains(result.Data.Cars, x => x.Id == 1);
            Assert.Contains(result.Data.Cars, x => x.Id == 3);
            Assert.DoesNotContain(result.Data.Cars, x => x.Id == 2);
        }

        [Fact]
        public async Task Should_Apply_All_Applicable_Discounts() {
            // Arrange
            var cars = new List<Car> {
                new() { Id = 1, Year = 1990, Price = 50000 },
                new() { Id = 2, Year = 1998, Price = 30000 },
                new() { Id = 3, Year = 2015, Price = 40000 }
            };

            _carRepository.GetAllCarsAsync(Arg.Any<CancellationToken>())
                .Returns(cars);

            // mocked only two cars in repo
            var query = new CalculateCarsDiscountsQuery(
                new List<int> { 1, 2, 3 }
            );

            // total price = 120000
            // > 2 cars => 3% = 3600
            // total > 100000 => 5% = 6000
            // old cars:
            // 50000 * 10% = 5000
            // 30000 * 10% = 3000
            // old total = 8000
            // grand total discount = 17600

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(120_000m, result.Data.TotalPrice);
            Assert.Equal(17_600m, result.Data.TotalDiscount);
        }
    }
}
