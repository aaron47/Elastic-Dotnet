using System.Text;
using Azure.Identity;
using Elastic_Dotnet.Swagger;
using ElasticDotnet.Application;
using ElasticDotnet.Domain.Config;
using ElasticDotnet.Infrastructure;
using ElasticDotnet.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var keyVaultUrl = new Uri(config.GetSection("KeyVaultUrl").Value!);

var azureCrendential = new DefaultAzureCredential(true);
builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCrendential);

// getting the keys from azure
var jwtSecret = builder.Configuration.GetSection("jwtsecret").Value!;
var azureConnectionString = builder.Configuration.GetSection("azureconnectionstring").Value!;
var encodeApi = builder.Configuration.GetSection("encodeapi").Value!;

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JWT:Issuer"],
        ValidAudience = config["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.Configure<PythonMicroserviceOptions>(options =>
{
    options.Url = encodeApi;
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<Secret>(options =>
{
    options.JwtSecret = jwtSecret;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(azureConnectionString,
        b => b.MigrationsAssembly("Elastic-Dotnet")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
