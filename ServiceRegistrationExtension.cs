using BlazorPrerenderHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorPrerenderHelper;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddPrerenderHelperForClient(this IServiceCollection services)
    {
        return services.AddScoped<ISSRService, ClientService>();
    }
    
    public static IServiceCollection AddPrerenderHelperForServer(this IServiceCollection services)
    {
        return services.AddScoped<ServerService>()
            .AddScoped<ISSRService>(sp => sp.GetRequiredService<ServerService>())
            .AddScoped<IPrerenderScriptGenerator>(sp => sp.GetRequiredService<ServerService>());
    }
}