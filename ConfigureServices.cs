using Microsoft.AspNetCore.Authentication;

namespace APIKeys_MinimalAPIs
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddConfigurationOptions(this IServiceCollection services)
        {
            services.AddOptions<ApiOptions>().BindConfiguration("Api");
            return services;
        }

        public static IServiceCollection AddApiKeyAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApiKeyAuthenticationHandler.SchemeName, policy =>
                {
                    policy.AddAuthenticationSchemes(ApiKeyAuthenticationHandler.SchemeName);
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationHandler.SchemeName, null);

            return services;
        }
    }
}
