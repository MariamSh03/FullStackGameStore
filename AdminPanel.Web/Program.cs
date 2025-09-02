using System.Text;
using System.Text.Json;
using AdminPanel.Bll.Configuration;
using AdminPanel.Bll.Constants;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Bll.Mapping;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Context;
using AdminPanel.Dal.Repositories;
using AdminPanel.Dal.Repositories.Implementations;
using AdminPanel.Entity;
using AdminPanel.Entity.Authorization;
using AdminPanel.Web.Authorization;
using AdminPanel.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestPDF;
using QuestPDF.Infrastructure;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine("Logs", "error-log-.txt"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true; // For readability
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString!));
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenericRepository<CommentEntity>, GenericRepository<CommentEntity>>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "GameStore API", Version = "v1" });

    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        },
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy.WithOrigins("https://localhost:7281")
             .AllowAnyMethod()
             .AllowAnyHeader();
    });

    options.AddPolicy(
       "AllowAngularApp",
       builder =>
       {
           builder.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
       });
});

// ?? Configure QuestPDF license
Settings.License = LicenseType.Community;

// Configure external auth HttpClient using configuration
var externalAuthConfig = builder.Configuration.GetSection("ExternalAuth").Get<ExternalAuthConfig>();
if (externalAuthConfig != null)
{
    if (!externalAuthConfig.IsValid())
    {
        var errors = externalAuthConfig.GetValidationErrors();
        throw new InvalidOperationException($"External Auth configuration is invalid: {string.Join(", ", errors)}");
    }

    builder.Services.AddHttpClient("ExternalAuthClient", client =>
    {
        client.BaseAddress = new Uri(externalAuthConfig.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(externalAuthConfig.TimeoutSeconds);
    });
}
else
{
    Log.Warning("External Auth configuration not found. External authentication will not be available.");
}

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["SecretKey"];

if (string.IsNullOrEmpty(secret))
{
    throw new InvalidOperationException("JWT Secret not configured in appsettings.json.");
}

// Add Identity
builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        ClockSkew = TimeSpan.Zero,
    };

    // Add detailed error logging
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
            Console.WriteLine($"Request Path: {context.Request.Path}");
            Console.WriteLine($"Authorization Header: {context.Request.Headers["Authorization"]}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var userId = context.Principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"JWT Token validated successfully for user: {userId}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"JWT Challenge triggered. Path: {context.Request.Path}");
            Console.WriteLine($"Authorization Header: {context.Request.Headers["Authorization"]}");
            return Task.CompletedTask;
        },
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));

    // Add permission-based policies for all permissions
    foreach (var permission in Permissions.AllPermissions)
    {
        options.AddPolicy($"RequirePermission.{permission}", policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});

// Register permission authorization handler
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Register role permission seeder
builder.Services.AddScoped<RolePermissionSeeder>();

// Register user seeder
builder.Services.AddScoped<UserSeeder>();

// Add configuration for external auth
builder.Services.Configure<ExternalAuthConfig>(builder.Configuration.GetSection("ExternalAuth"));

// HttpClient for external auth is already configured above with the "ExternalAuthClient" name

// Register external auth service
builder.Services.AddScoped<IExternalAuthService, ExternalAuthService>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register custom authorization services
builder.Services.AddScoped<IUserClaimsPrincipalFactory<UserEntity>, ApplicationClaimsPrincipalFactory>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ApplicationAuthorizationPolicyProvider>();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAngularApp");

// app.UseMiddleware<TotalGamesHeaderMiddleware>(); // Temporarily disabled due to DB timeout
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = "swagger";
});

app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<RolePermissionSeeder>();
    await seeder.SeedRolePermissionsAsync();

    var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
    await userSeeder.SeedDefaultUsersAsync();
}

app.Run();
