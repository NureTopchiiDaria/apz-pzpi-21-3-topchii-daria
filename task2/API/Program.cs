using System;
using System.Text;
using Core.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication1.Controllers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; // Получаем IConfiguration из параметров метода CreateBuilder

// Добавляем IConfiguration в сервисы, чтобы иметь доступ к нему в других частях кода
AdminSettings adminSettings = builder.Configuration.GetSection("AdminSettings").Get<AdminSettings>() ??
                              throw new Exception("Admin configuration is missing");

// Add services to the container.   

builder.Services.AddControllers();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddRazorPages();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestAPI", Version = "v1" });
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Basic authentication"
    });

    // Добавляем требование базовой аутентификации в Swagger для всех методов контроллера, требующих аутентификации
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
            },
            Array.Empty<string>()
        }
    });

    // Используйте эту строку, чтобы включить поддержку XML-комментариев
   //  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "YourApiDocumentation.xml"));
});
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ??
                          throw new Exception("JWT configuration is missing");
builder.Services.AddSingleton(adminSettings);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,

            ValidAudience = jwtSettings.ValidAudience,
            ValidIssuer = jwtSettings.ValidIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

// Конфигурируем настройки Email и App
builder.Services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

// Добавляем зависимости для BLL
BLL.DependencyRegistrar.ConfigureServices(builder.Services, configuration);

var app = builder.Build();

// Конфигурируем Swagger для development окружения
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestAPI V1");
    });
}

// Конфигурируем HTTP request pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Login}/{action=Index}");

app.MapControllerRoute(
    name: "register",
    pattern: "{controller=Registration}/{action=Index}");

app.Run();

