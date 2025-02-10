using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserSettingsApi;
using UserSettingsApi.Data;
using UserSettingsApi.DatabaseOperations.Repository.ChatsRepository;
using UserSettingsApi.Managers.ChatsManager;
using UserSettingsApi.UserAccessor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://localhost:7500")
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
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMongoDB
    (builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")!,
    (builder.Configuration.GetValue<string>("MongoDbSettings:DatabaseName")!));
});


builder.AddServices();
var app = builder.Build();

using var scope = app.Services.CreateScope();
var chatsManager = scope.ServiceProvider.GetRequiredService<IChatsManager>();

app.MapChatsEndpoints(chatsManager);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

DataBaseGenerator.Generate(app);
app.Run();


