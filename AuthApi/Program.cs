using AuthApi;
using AuthApi.Cookies;
using AuthApi.Data;
using AuthApi.DataBaseOperations.Commands.RoleCommands;
using AuthApi.DataBaseOperations.Commands.UserAccountsCommands;
using AuthApi.DataBaseOperations.Repository.RoleRepository;
using AuthApi.DataBaseOperations.Repository.UserAccountsRepository;
using AuthApi.Dto;
using AuthApi.Jwt;
using AuthApi.Managers.AdminManager;
using AuthApi.Managers.UserManager;
using AuthApi.UserAccessor;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://localhost:7500", "http://192.168.0.228:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer")!,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience")!,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:TokenString")!)),
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Request.Cookies.TryGetValue("ChatApp", out var accessToken);
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"Token validated for user: {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.AddServices();
var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using var scope = app.Services.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();
var adminManager = scope.ServiceProvider.GetRequiredService<IAdminManager>();

app.MapUserEndpoints(userManager)
    .MapAdminEndpoints(adminManager);
app.GenerateDatabase();
app.Run();

