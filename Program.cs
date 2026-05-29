
using APIKeys_MinimalAPIs;
using System.Text.Json.Serialization;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddValidatorsFromAssemblyContaining<OrderValidator>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<ApiKeySecurityTransformer>();
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling =
        JsonNumberHandling.Strict;
});

builder.Services
    .AddConfigurationOptions()
    .AddApiKeyAuthorization();

var app = builder.Build();

app.MapProductsEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Api v1");
    });
}

app.UseHttpsRedirection();

app.Run();
