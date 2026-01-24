using Core.Services.Interfaces;
using Core.Services.Implementations;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using System.Data;
using SQLitePCL;
using Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
Batteries.Init();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. Example: X-API-Key: {key}",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-API-Key", 
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Register a scoped IDbConnection for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var conn = new SqliteConnection("Data Source=data/UserDB.db;");
    conn.Open(); // Important: Dapper needs an open connection
    return conn;
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApiClientRepository, ApiClientRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IApiClientService, ApiClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
