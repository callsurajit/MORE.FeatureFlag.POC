using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Azure.Identity;
using System.Text.Json.Serialization;
using Microsoft.FeatureManagement.FeatureFilters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFeatureManagement()
                .AddFeatureFilter<TimeWindowFilter>();
// Retrieve the app config connection string
var connectionString = Environment.GetEnvironmentVariable("AppConfig");
// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
                    options.Connect(connectionString).UseFeatureFlags());
builder.Services.AddAzureAppConfiguration();

builder.Services.AddControllers()
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;  
                });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "POC - Feature Flag implementation using Azure",
        Description = "Implementing the feature manager using Azure",
        TermsOfService = new Uri("https://example.com/terms"),
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "POC - Feature Flag implementation using Azure App Config v1");
    });
}

app.UseHttpsRedirection();
app.UseAzureAppConfiguration();
app.UseAuthorization();
app.MapControllers();
app.Run();
