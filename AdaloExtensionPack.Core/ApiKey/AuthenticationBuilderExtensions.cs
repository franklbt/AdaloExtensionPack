using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AdaloExtensionPack.Core.ApiKey
{
    internal static class AuthenticationBuilderExtensions
    {
        internal static IServiceCollection AddApiKey(this IServiceCollection services,
            Action<ApiKeyAuthenticationOptions> optionsBuilder)
        {
            services.Configure<MvcOptions>(c =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                c.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                    options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                })
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationOptions.DefaultScheme, optionsBuilder);

            return services;
        }
    }
}
