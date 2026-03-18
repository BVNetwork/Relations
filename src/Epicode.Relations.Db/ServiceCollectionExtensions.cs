using System;
using Microsoft.Extensions.DependencyInjection;

namespace EPiCode.Relations.Db;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRelationsDb(
        this IServiceCollection services,
        Action<RelationsDbOptions> configure = null)
    {
        services.AddOptions<RelationsDbOptions>()
            .BindConfiguration("RelationsDb");

        if (configure != null)
            services.Configure<RelationsDbOptions>(configure);

        return services;
    }
}
