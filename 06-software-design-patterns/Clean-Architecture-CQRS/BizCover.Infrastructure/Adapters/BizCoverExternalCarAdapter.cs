using BizCover.Infrastructure.Mappers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using DomainCar = BizCover.Domain.Entities.Car;
using ExternalCar = BizCover.Repository.Cars.Car;

namespace BizCover.Infrastructure.Adapters {
    public class BizCoverExternalCarAdapter : Application.Interfaces.ICarRepository {
        private readonly Repository.Cars.ICarRepository _externalCarRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<BizCoverExternalCarAdapter> _logger;

        public BizCoverExternalCarAdapter(Repository.Cars.ICarRepository externalCarRepository
            , IMemoryCache cache
            , ILogger<BizCoverExternalCarAdapter> logger) {
            _externalCarRepository = externalCarRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<DomainCar>> GetAllCarsAsync(CancellationToken cancellationToken = default) { 
            
            if(_cache.TryGetValue("AllCars", out IEnumerable<DomainCar> cachedCars) && cachedCars != null) {
                _logger.LogInformation("Returning cached cars: {@Cars}", JsonSerializer.Serialize(cachedCars));
                return cachedCars;
            }

            var data = BizoverExternalCarMapper.MapToDomainCars(await _externalCarRepository.GetAllCars());
            _cache.Set("AllCars", data, TimeSpan.FromSeconds(10));
            _logger.LogInformation("Fetched all cars from external repository: {@Cars}", JsonSerializer.Serialize(data));
            return data;
        }
        public async Task AddAsync(DomainCar car, CancellationToken cancellationToken = default) {
            var mappedcar = BizoverExternalCarMapper.MapToExternalCar(car);
            _logger.LogInformation("Adding car: {@Car} to external repository", JsonSerializer.Serialize(mappedcar));
            car.Id = await _externalCarRepository.Add(mappedcar);
        }
        public async Task UpdateAsync(DomainCar car, CancellationToken cancellationToken = default) {
            var mappedcar = BizoverExternalCarMapper.MapToExternalCar(car);
            _logger.LogInformation("Updating car: {@Car} in external repository", JsonSerializer.Serialize(mappedcar));
            await _externalCarRepository.Update(mappedcar);
        }
    }
}
