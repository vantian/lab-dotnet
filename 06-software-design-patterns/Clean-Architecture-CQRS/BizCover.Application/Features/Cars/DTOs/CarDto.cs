using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Application.Features.Cars.DTOs {
    public class CarDto {

        public int?     Id                  { get; set; } = default;
        public int?     Year                { get; set; } = default;
        public decimal? Price               { get; set; } = default;
        public string?  Model               { get; set; } = default;
        public string?  Make                { get; set; } = default;
        public string?  CountryManufactured { get; set; } = default;
        public string?  Colour              { get; set; } = default;
    }
}
