using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Infrastructure.Mappers {
    internal static class BizoverExternalCarMapper {
        
        public static Domain.Entities.Car MapToDomainCar(Repository.Cars.Car externalCar) {
            return new Domain.Entities.Car {
                Id                  = externalCar.Id,
                Year                = externalCar.Year,
                Price               = externalCar.Price,
                Model               = externalCar.Model,
                Make                = externalCar.Make,
                CountryManufactured = externalCar.CountryManufactured,
                Colour              = externalCar.Colour
            };
        }
        public static Repository.Cars.Car MapToExternalCar(Domain.Entities.Car domainCar) {
            return new Repository.Cars.Car {
                Id                  = domainCar.Id,
                Year                = domainCar.Year,
                Price               = domainCar.Price,
                Model               = domainCar.Model,
                Make                = domainCar.Make,
                CountryManufactured = domainCar.CountryManufactured,
                Colour              = domainCar.Colour
            };
        }

        public static IEnumerable<Domain.Entities.Car> MapToDomainCars(IEnumerable<Repository.Cars.Car> externalCars) {
            foreach (var externalCar in externalCars) {
                yield return MapToDomainCar(externalCar);
            }
        }
    }
}
