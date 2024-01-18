using System.Text;
using Azure.Identity;
using ElasticDotnet.Application;
using ElasticDotnet.Domain.Config;
using ElasticDotnet.Infrastructure;
using ElasticDotnet.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var keyVaultUrl = new Uri(config.GetSection("KeyVaultUrl").Value!);


var azureCrendential = new DefaultAzureCredential(true);
builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCrendential);

// getting the keys from azure
var elasticCloudId = builder.Configuration.GetSection("elasticcloudid").Value!;
var elasticPassword = builder.Configuration.GetSection("elasticpassword").Value!;
var jwtSecret = builder.Configuration.GetSection("jwtsecret").Value!;
var connectionString = config.GetConnectionString("SqlServer");

builder.Services
    .AddApplication(elasticCloudId, elasticPassword)
    .AddInfrastructure(connectionString!)
    .AddPresentation();

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

builder.Services.Configure<PythonMicroserviceOptions>(builder.Configuration.GetSection("PythonMicroservice"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<Secret>(options =>
{
    options.JwtSecret = jwtSecret;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
