using System;
using Microsoft.Extensions.DependencyInjection;

namespace AdaloExtensionPack.Core.Smtp;

public static class SmtpServiceCollectionExtension
{
    public static IServiceCollection AddSmtp(this IServiceCollection services, Action<SmtpOptions> optionsFactory)
    {
        services.Configure(optionsFactory);
        services.AddScoped<ISmtpService, SmtpService>();
        return services;
    }
}