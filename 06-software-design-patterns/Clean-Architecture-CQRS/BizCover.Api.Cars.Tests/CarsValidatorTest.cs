using BizCover.Api.Cars.Controllers;
using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.Commands.AddCar;
using BizCover.Application.Features.Cars.Commands.UpdateCar;
using BizCover.Application.Features.Cars.DTOs;
using BizCover.Application.Features.Cars.Queries.GetAllCars;
using BizCover.Application.Features.Cars.Queries.GetCarsDiscounts;
using FluentValidation.TestHelper;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BizCover.Api.Cars.Tests {
    public class CarsValidatorTest {
        private readonly IMediator _mediator;
        private readonly AddCarValidator _addCarValidator;
        private readonly UpdateCarValidator _updateCarValidator;
        public CarsValidatorTest() {
            _mediator = Substitute.For<IMediator>();
            _addCarValidator = new AddCarValidator();
            _updateCarValidator = new UpdateCarValidator();
        }

        [Theory]
        [InlineData(1907)]
        [InlineData(1800)]
        [InlineData(0)]
        public void Should_Have_Error_When_Year_Is_more_than_or_equal_1908(int year) {
            var command = new AddCarCommand (
                new CarDto {
                    Make = "Toyota",
                    Model = "Corolla",
                    Year = year,
                    Price = 10000,
                    CountryManufactured = "Japan",
                    Colour = "White"
                }
            );

            var result = _addCarValidator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("car.Year");
        }
    }
}
