using BizCover.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.DTOs {
    public class CarDiscountDto {

        public CarDiscountDto() { }
        public CarDiscountDto(List<Car> cars) {
            Cars = cars.Select(car => new CarDto {
                Id                  = car.Id,
                Make                = car.Make,
                Model               = car.Model,
                Year                = car.Year,
                Price               = car.Price,
                CountryManufactured = car.CountryManufactured,
                Colour              = car.Colour
            }).ToList();
        }

        public IList<CarDto> Cars { get; set; } = new List<CarDto>();
        public decimal TotalDiscount { get; set; } = 0;
        public int     TotalOldCars  => Cars.Where(car => car.Year < 2000).Count();
        public decimal TotalPrice    => Cars.Sum(car => car.Price ?? 0);
        public decimal TotalAmount   => TotalPrice - TotalDiscount;
    }
}
