using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.Commands.AddCar {
    public class AddCarValidator : AbstractValidator<AddCarCommand> {
        public AddCarValidator() {
            RuleFor(x => x.car                    ).NotNull().WithMessage("Car cannot be null.");
            RuleFor(x => x.car.Make               ).NotEmpty()                .WithMessage("Make cannot be empty.");
            RuleFor(x => x.car.Model              ).NotEmpty()                .WithMessage("Model cannot be empty.");
            RuleFor(x => x.car.Year               ).GreaterThanOrEqualTo(1908).WithMessage("Year is invalid.");
            RuleFor(x => x.car.Price              ).GreaterThanOrEqualTo(0)   .WithMessage("Price must be greater or equal than zero.");
            RuleFor(x => x.car.CountryManufactured).NotEmpty()                .WithMessage("Country Manufactured cannot be empty.");
            RuleFor(x => x.car.Colour             ).NotEmpty()                .WithMessage("Colour cannot be empty.");
        }
    }
}