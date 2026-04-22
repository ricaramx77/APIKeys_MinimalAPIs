using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace APIKeys_MinimalAPIs
{
    public class ApiKeySecurityTransformer : IOpenApiDocumentTransformer
    {
        private readonly IAuthenticationSchemeProvider
            _authenticationSchemeProvider;

        public ApiKeySecurityTransformer(
            IAuthenticationSchemeProvider authenticationSchemeProvider)
        {
            _authenticationSchemeProvider = authenticationSchemeProvider;
        }

        public async Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken
        )
        {
            var authenticationSchemes = await _authenticationSchemeProvider
                .GetAllSchemesAsync();

            if (authenticationSchemes.Any(s =>
                s.Name == ApiKeyAuthenticationHandler.SchemeName))
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??=
                    new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes.TryAdd(
                    ApiKeyAuthenticationHandler.SchemeName,
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Name = "api-key",
                        Description = "API key"
                    }
                );

                document.Security ??= [];
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(
                        ApiKeyAuthenticationHandler.SchemeName,
                        document
                        )] = []
                });

            }
        }
    }
}
