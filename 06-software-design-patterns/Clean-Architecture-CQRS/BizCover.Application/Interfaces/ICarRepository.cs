using BizCover.Domain.Entities;

namespace BizCover.Application.Interfaces {
    public interface ICarRepository {
        Task<IEnumerable<Car>> GetAllCarsAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Car car, CancellationToken cancellationToken = default);
        Task UpdateAsync(Car car, CancellationToken cancellationToken = default);
    }
}
