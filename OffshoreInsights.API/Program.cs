using System.Text.Json.Serialization;
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
    options.AddSchemaTransformer<StringEnumSchemaTransformer>();
    options.AddOperationTransformer<PathParameterExamplesTransformer>();
    options.AddOperationTransformer<VesselPositionExamplesTransformer>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();