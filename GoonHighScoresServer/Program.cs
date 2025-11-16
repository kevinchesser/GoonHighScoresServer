using GoonHighScoresServer.Services;
using GoonHighScoresServer.Interfaces;
using GoonHighScoresServer.Repositories;
using Microsoft.OpenApi;
using static GoonHighScoresServer.Repositories.HighScoreSQLiteRepository;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

ConfigureControllers();
ConfigureDatabases();
ConfigureServices();

void ConfigureControllers()
{
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
});
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "HighScores API", Version = "v1" });
});
}
void ConfigureDatabases()
{
    services.Configure<HighScoreSQLiteRepositoryOptions>(configuration.GetSection("Databases:SQLite"));
    services.AddSingleton<IHighScoreRepository, HighScoreSQLiteRepository>();
}

void ConfigureServices()
{
    services.AddSingleton<IHighScoreService, HighScoreService>();
}


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("v1/swagger.json", "HighScores API");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
