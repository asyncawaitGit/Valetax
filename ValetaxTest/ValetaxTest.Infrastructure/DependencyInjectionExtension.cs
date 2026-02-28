using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ValetaxTest.Application.Interfaces;
using ValetaxTest.Infrastructure.Data;
using ValetaxTest.Infrastructure.Repositories;

namespace ValetaxTest.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<ValetaxTestDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ValetaxTestDbContext).Assembly.FullName)));

            services.AddScoped<INodeRepository, NodeRepository>();
            services.AddScoped<IExceptionJournalRepository, ExceptionJournalRepository>();

            return services;
        }
    }
}
