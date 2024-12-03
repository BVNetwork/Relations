using System;
using System.Linq;
using EPiServer.Authorization;
using EPiServer.Shell.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EPiCode.Relations.Infrastructure;

public static class ServiceCollectionExtensions
{
    private static readonly Action<AuthorizationPolicyBuilder> DefaultPolicy = p => p.RequireRole(Roles.CmsEditors, Roles.CmsAdmins);

    public static IServiceCollection AddRelations(
        this IServiceCollection services)
    {
        return services.AddRelations(DefaultPolicy);
    }

    public static IServiceCollection AddRelations(
        this IServiceCollection services, Action<AuthorizationPolicyBuilder> configurePolicy)
    {
        AddModule(services);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Constants.PolicyName, configurePolicy);
        });

        return services;
    }

    private static void AddModule(IServiceCollection services)
    {
        services.Configure<ProtectedModuleOptions>(
            pm =>
            {
                if (!pm.Items.Any(i => i.Name.Equals(Constants.ModuleName, StringComparison.OrdinalIgnoreCase)))
                {
                    pm.Items.Add(new ModuleDetails {Name = Constants.ModuleName});
                }
            });
    }
}