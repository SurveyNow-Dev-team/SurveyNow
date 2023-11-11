using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace SurveyNow;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, string? jwtKey)
    {
        services.AddCors(options =>
            {
                options.AddPolicy(name: "_publicPolicy",
                    //Define cors URL 
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            }
        );
        services.AddControllers()
            //allow enum string value in swagger and front-end instead of int value
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        //Add authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = _configuration["Jwt:Issuer"],
                    //ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            }
        );

        //Authorization
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => { policy.RequireRole(Role.Admin.ToString()); });
        });

        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Survey Now API", Version = "v1", Description = "ASP NET core API for Survey Now project."
                });
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            option.IncludeXmlComments(xmlPath);
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.AddDistributedMemoryCache();
        services.AddSession();

        return services;
    }
}