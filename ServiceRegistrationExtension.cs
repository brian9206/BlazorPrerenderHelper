using BlazorPrerenderHelper.Models;
using BlazorPrerenderHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorPrerenderHelper;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddPrerenderHelperForClient(this IServiceCollection services, Action<PrerenderHelperOptions>? configure = null)
    {
        AddPrerenderHelperOptions(services, configure);
        return services.AddScoped<ISSRService, ClientService>();
    }
    
    public static IServiceCollection AddPrerenderHelperForServer(this IServiceCollection services, Action<PrerenderHelperOptions>? configure = null)
    {
        AddPrerenderHelperOptions(services, configure);
        return services.AddScoped<ServerService>()
            .AddScoped<ISSRService>(sp => sp.GetRequiredService<ServerService>())
            .AddScoped<IPrerenderScriptGenerator>(sp => sp.GetRequiredService<ServerService>());
    }

    private static void AddPrerenderHelperOptions(IServiceCollection services, Action<PrerenderHelperOptions>? configure)
    {
        var options = new PrerenderHelperOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);
    }
}