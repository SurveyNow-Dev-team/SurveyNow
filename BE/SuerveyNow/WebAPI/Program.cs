using System.Text.Json.Serialization;
using Application.Configurations;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Add dependency injection
var configuration = builder.Configuration.Get<AppConfiguration>();
configuration.DatabaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
configuration.Key = builder.Configuration["JwtSettings:Key"];
builder.Services.AddDependency(configuration.DatabaseConnection);
builder.Services.AddSingleton(configuration);

//allow enum string value in swagger and front-end instead of int value
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();