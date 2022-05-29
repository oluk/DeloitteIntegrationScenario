using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CITYAPI.Data;
using CITYAPI.Interfaces;
using CITYAPI.Repositories;
using CITYAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CITYAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}