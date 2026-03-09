using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Domain.Entities {
    public class Car {
        public int     Id                  { get; set; } = 0;
        public int     Year                { get; set; } = 0;
        public decimal Price               { get; set; } = 0;
        public string  Model               { get; set; } = string.Empty;
        public string  Make                { get; set; } = string.Empty;
        public string  CountryManufactured { get; set; } = string.Empty;
        public string  Colour              { get; set; } = string.Empty;

        public Car() { }
        public Car(int year, decimal price, string model, string make, string countryManufactured, string colour) {

            if (string.IsNullOrEmpty(make))
                throw new ArgumentException("Make cannot be empty.");

            if (string.IsNullOrEmpty(model))
                throw new ArgumentException("Model cannot be empty.");

            if (year < 1885) //fun fact 1885 is the year when the first car was invented by Karl Benz. https://en.wikipedia.org/wiki/History_of_the_automobile
                throw new ArgumentException("Year is invalid.");

            if (price < 0)
                throw new ArgumentException("Price must be greater or equal than zero.");

            if (string.IsNullOrEmpty(countryManufactured))
                throw new ArgumentException("Country Manufactured cannot be empty.");

            if (string.IsNullOrEmpty(colour))
                throw new ArgumentException("Colour cannot be empty.");

            Model = model.Trim();
            Year = year;
            Price = price;
            Make = make.Trim();
            CountryManufactured = countryManufactured.Trim();
            Colour = colour.Trim();
        }
    }
}
