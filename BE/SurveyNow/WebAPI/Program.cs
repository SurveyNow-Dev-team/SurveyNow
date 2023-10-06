using System.Net;
using Application.Configurations;
using Application.Utils;
using Infrastructure;
using SurveyNow;
using SurveyNow.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppConfiguration>();
if (configuration != null)
{
    configuration.DatabaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    configuration.Key = builder.Configuration["JwtSettings:Key"];
    builder.Services.AddDependency(configuration.DatabaseConnection);
    builder.Services.AddApiServices(configuration.Key);
    builder.Services.AddSingleton(configuration);
}

// Add momo configuration
builder.Services.Configure<MomoConfig>(builder.Configuration.GetSection("MomoAPI"));

//Config Https redirect port for production
/*if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
        options.HttpsPort = 443;
    });
}*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "SurveyNow V1");
    });

}

/*if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}*/

// Configure the HTTPS redirection middleware with the obtained port
// app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();