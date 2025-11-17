using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIMAPI.Data;
using System.Text;
using SIMAPI.Business.Interfaces;
using SIMAPI.Business.Services;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Repository.Repositories;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using SIMAPI;
using SIMAPI.Business.Helper;
//using Serilog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
//#region Logging

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to files
//    .MinimumLevel.Debug()
//    .CreateLogger();

//builder.Host.UseSerilog();

//#endregion

#region Database Configuration

string connectionString = builder.Configuration["AppSettings:SimDBConnection"];
//string connectionString = "Data Source=WIN-4AO2GAUSMUQ;Initial Catalog=GlobalSims;User ID=sa;Password=$June$2024*06£05$";
builder.Services.AddDbContext<SIMDBContext>(options => options.UseSqlServer(connectionString));


#endregion


#region AutoMapper

builder.Services.AddAutoMapper(typeof(Program));

#endregion

// Add services to the container.
#region Add Services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBulkUploadService, BulkUploadService>();
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
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<ITopupService, TopupService>();
builder.Services.AddScoped<ITopupWalletService, TopupWalletService>();
builder.Services.AddScoped<IMixMatchGroupService, MixMatchGroupService>();
builder.Services.AddScoped<IRetailerService, RetailerService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();


builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IBulkUploadRepository, BulkUploadRepository>();
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
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IManagementRepository, ManagementRepository>();
builder.Services.AddScoped<ITopupRepository, TopupRepository>();
builder.Services.AddScoped<IMixMatchGroupRepository, MixMatchGroupRepository>();
builder.Services.AddScoped<IRetailerRepository, RetailerRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();


#endregion

var allowedOrigin = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("myAppCors", policy =>
    {
        policy.WithOrigins(allowedOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.FromSeconds(30)
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

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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
app.UseCors("myAppCors");
app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();
//app.UseSerilogRequestLogging(); // Automatically log HTTP requests

app.MapControllers();
app.Run();
//try
//{
//    Log.Information("Starting the application...");
//    app.Run();
//}
//catch (Exception ex)
//{
//    Log.Fatal(ex, "Application terminated unexpectedly.");
//}
//finally
//{
//    Log.CloseAndFlush();
//}
