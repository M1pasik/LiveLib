using System.Text;
using LiveLib.Application;
using LiveLib.CacheProvider;
using LiveLib.Database;
using LiveLib.JwtProvider;
using LiveLib.PasswordHasher;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
var services = builder.Services;
var config = builder.Configuration;

services.AddApplication(config);

services.AddContext(config);

services.AddJwtProvider(config);

services.AddPasswordHasher(config);

services.AddCache(config);

var JwtOptions = config.GetSection("JwtOptions");
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtOptions["Issuer"],
        ValidAudience = JwtOptions["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions["SecretKey"]!)),
        ClockSkew = TimeSpan.Zero
    };

    opt.Events = new()
    {
        OnMessageReceived = context =>
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? string.Empty;

            //var token = context.Request.Cookies[JwtOptions["CookieName"] ?? "RefreshToken"];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});

services.AddOpenApi();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LiveLib API ",
        Version = "v1",
        Description = "API for LiveLib",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        // localhost:5500 - адрес, который находится в загроовке запроса "Origin".
        // в текущей конфигурации cookie same-site=lax, cookie будут передаваться в кросс-доменных запросах ТОЛЬКО при GET запросах.
        // например, если клиент запущен на localhost:5500 и сервер на localhost:5001, то cookie будут передаваться и при POST запросах.
        // если клиент запущен на 127.0.0.1:5500 и сервер на localhost:5001, то возникнет ошибка  CORS, т.к. 127.0.0.1:5500 != localhost:5500,
        // можно установить параметр same-site=none, тогда cookie будут передаваться на любой домен и при этом cookie должен иметь флаг secure,
        // если указать same-site=none без secure, браузер проигнорирует такой cookie.
        builder.WithOrigins("http://localhost:5500")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});



var app = builder.Build();

app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception is Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync($"An unexpected error occurred:{exception.Message}");
        }
    });
});

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<PostgresDatabaseContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception)
    {
        throw;
    }
}

Console.WriteLine($"Is Development: {app.Environment.IsDevelopment()}");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}

app.Run();
