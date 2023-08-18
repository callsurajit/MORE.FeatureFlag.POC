using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Microsoft.FeatureManagement.FeatureFilters;
using MORE.FeatureFlag.POC.Data;
using Microsoft.EntityFrameworkCore;

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

// Register SQL database configuration context as services.
builder.Services.AddDbContext<MOREPOCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddControllers();
//                .AddJsonOptions(option =>
//                {
//                    option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;  
//                });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "POC - Feature Flag implementation using Azure/Questionnaire",
        Description = "Implementing the feature manager using Azure/Questionnaire",
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
