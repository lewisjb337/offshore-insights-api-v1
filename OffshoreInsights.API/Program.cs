using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using OffshoreInsights.API.Filters;
using OffshoreInsights.API.OpenApi;
using OffshoreInsights.Application.IoC;
using OffshoreInsights.Infrastructure.IoC;
using OffshoreInsights.Persistence.IoC;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddPersistence(connectionString);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<ApiKeySecurityTransformer>();
    options.AddSchemaTransformer<StringEnumSchemaTransformer>();
    options.AddOperationTransformer<PathParameterExamplesTransformer>();
    options.AddOperationTransformer<VesselPositionExamplesTransformer>();
});

var app = builder.Build();

// Railway (and most reverse proxies) terminate TLS at the edge and forward plain
// HTTP to the container. Without this, the app thinks it is running on HTTP, the
// generated OpenAPI spec gets an http:// server URL, and the browser blocks every
// Scalar request as mixed content (no HTTP status code, just "Failed to fetch").
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
};
forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();