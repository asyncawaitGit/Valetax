using Microsoft.Extensions.DependencyInjection;
using ValetaxTest.Application.Services;

namespace ValetaxTest.Application;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITreeService, TreeService>();

        return services;
    }
}
