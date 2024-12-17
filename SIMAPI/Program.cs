//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIMAPI.Business.Interfaces;
using SIMAPI.Business.Services;
using SIMAPI.Data;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Repository.Repositories;
using Serilog;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

//#region Logging

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to files
    .MinimumLevel.Debug()
    .CreateLogger();

builder.Host.UseSerilog();

//#endregion

#region Database Configuration


//string connectionString = "Data Source=WIN-4AO2GAUSMUQ;Initial Catalog=GlobalSims;User ID=sa;Password=$June$2024*06£05$";
//string connectionString = "Data Source=.;Initial Catalog=SIMDB;Integrated Security=True;TrustServerCertificate=True";
string connectionString = "Data Source=WIN-NTOD73IHIC7\\SA;Initial Catalog=SIMDB;Integrated Security=True;";
builder.Services.AddDbContext<SIMDBContext>(options => options.UseSqlServer(connectionString));


#endregion

#region AutoMapper

builder.Services.AddAutoMapper(typeof(Program));

#endregion



#region Add Services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDownloadService, DownloadService>();
builder.Services.AddScoped<ILookUpService, LookUpService>();
builder.Services.AddScoped<INetworkService, NetworkService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IOnFieldService, OnFieldService>();
builder.Services.AddScoped<IDashboardService, DasboardService>();
builder.Services.AddScoped<ISimService, SimService>();
builder.Services.AddScoped<ICommissionStatementService, CommissionStatementService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();


builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<ILookUpRepository, LookUpRepository>();
builder.Services.AddScoped<INetworkRepository, NetworkRepository>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IOnFieldRepository, OnFieldRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<ISimRepository, SimRepository>();
builder.Services.AddScoped<ICommissionStatementRepository, CommissionStatementRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();


#endregion



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

#endregion

#region Add Swagger Autentication

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SIM API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

#endregion



var app = builder.Build();


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
        RequestPath = "/Resources"
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseSerilogRequestLogging(); // Automatically log HTTP requests

app.MapControllers();

try
{
    Log.Information("Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}

