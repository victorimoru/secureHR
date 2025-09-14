using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureHR.Application.MessageBroker;
using SecureHR.Core.Repositories;
using SecureHR.Infrastructure.Data;
using SecureHR.Infrastructure.Repositories;

namespace SecureHR.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IIdempotencyRepository, SqlIdempotencyRepository>();

            services.AddSingleton<IMessageBus>(provider =>
            {
                var busConnectionString = configuration.GetConnectionString("AzureServiceBus");
                return new AzureServiceBusMessageBus(busConnectionString);
            });

            return services;
        }
    }
}
