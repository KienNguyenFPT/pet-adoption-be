using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer;
using BusinessLogicLayer.Commons;
using BusinessLogicLayer.IRepositories;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.Services;
using DataAccessObjects.Mappers;
using DataAccessObjects.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessObjects {
    public static class DependencyInjections {
        public static IServiceCollection AddInfrastructuresServices(this IServiceCollection services, string DatabaseConnection) {
            services.AddScoped<IShelterRepo, ShelterRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ICurrentTimeServices, CurrentTimeServices>();
            services.AddScoped<IClaimServices, ClaimServices>();
            services.AddScoped<IShelterServices, ShelterServices>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventRepo, EventRepo>();
            services.AddScoped<IEventEnrollmentRepo, EventEnrollmentRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IPetRepo, PetRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationServices>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPetServices, PetServices>();

            services.AddScoped<IPetImageRepo, PetImageRepo>();
            services.AddScoped<IPetImageServices, PetImageServices>();

            services.AddScoped<IEventImageRepo, EventImageRepo>();
            services.AddScoped<IEventImagesService, EventImagesService>();

            services.AddScoped<IAdoptionRepo, AdoptionRepo>();
            services.AddScoped<IAdoptionServices, AdoptionServices>();

            services.AddScoped<IDonationRepo, DonationRepo>();
            services.AddScoped<IDonationServices, DonationServices>();
            services.AddScoped<IHealthRepo, HealthRepo>();
            services.AddScoped<IHealthServices, HealthServices>();


            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").
                Build();
            var connectionString = configuration.GetConnectionString("AzureConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'AzureConnection' not found.");
            }


            services.AddDbContext<AppDBContext>(opts => {
                /* opts.UseSqlServer(DatabaseConnection);*/
                opts.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    );
                });
                opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);

            return services;
        }
    }
}
