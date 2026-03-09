using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizCover.Infrastructure.Persistences {
    public static class DependencyInjection {

        public static void RegisterServices(IServiceCollection services) {
            services.AddScoped<Repository.Cars.ICarRepository, Repository.Cars.CarRepository>();
            services.AddScoped<Application.Interfaces.ICarRepository, Adapters.BizCoverExternalCarAdapter>();
            services.AddMemoryCache();
        }
    }
}
