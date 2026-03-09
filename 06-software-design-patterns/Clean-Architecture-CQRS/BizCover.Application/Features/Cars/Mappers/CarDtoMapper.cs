using BizCover.Application.Features.Cars.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.Mappers { 
    internal static class CarDtoMapper {
        public static Car MapToDomainCar(CarDto carDto) {
            return new Car {
                Id                  = carDto.Id                  ?? default,
                Year                = carDto.Year                ?? default,
                Price               = carDto.Price               ?? default,
                Model               = carDto.Model               ?? string.Empty,
                Make                = carDto.Make                ?? string.Empty,
                CountryManufactured = carDto.CountryManufactured ?? string.Empty,
                Colour              = carDto.Colour              ?? string.Empty
            };
        }
        public static CarDto MapToDTO(Car domainCar) {
            return new CarDto {
                Id                  = domainCar.Id,
                Year                = domainCar.Year,
                Price               = domainCar.Price,
                Model               = domainCar.Model,
                Make                = domainCar.Make,
                CountryManufactured = domainCar.CountryManufactured,
                Colour              = domainCar.Colour
            };
        }
    }
    }