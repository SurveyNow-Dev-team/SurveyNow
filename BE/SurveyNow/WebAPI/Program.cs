using Application.Configurations;
using Infrastructure;
using SurveyNow;
using SurveyNow.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//Add dependency injection
var configuration = builder.Configuration.Get<AppConfiguration>();
if (configuration != null)
{
    configuration.DatabaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    configuration.Key = builder.Configuration["JwtSettings:Key"];
    builder.Services.AddDependency(configuration.DatabaseConnection);
    builder.Services.AddApiServices(configuration.Key);
    builder.Services.AddSingleton(configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();