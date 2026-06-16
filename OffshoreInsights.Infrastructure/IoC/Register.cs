using Microsoft.Extensions.DependencyInjection;
using OffshoreInsights.Application.Features.Account.Abstractions;
using OffshoreInsights.Application.Features.ApiKeys.Abstractions;
using OffshoreInsights.Application.Features.Buoys.Abstractions;
using OffshoreInsights.Application.Features.Geofences.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Weather;
using OffshoreInsights.Application.Features.Webhooks.Abstractions;
using OffshoreInsights.Infrastructure.Repositories;
using OffshoreInsights.Infrastructure.Services;

namespace OffshoreInsights.Infrastructure.IoC;

public static class Register
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IApiKeyValidator, ApiKeyValidator>();
        services.AddScoped<IAccountRepository, AccountData>();
        services.AddScoped<IBuoysData, BuoysData>();
        services.AddScoped<IGeofencesData, GeofencesData>();
        services.AddScoped<IVesselsData, VesselsData>();
        services.AddScoped<IWebhooksData, WebhooksData>();

        services.AddHttpClient("OpenMeteo");
        services.AddScoped<IWeatherService, OpenMeteoWeatherService>();
    }
}
