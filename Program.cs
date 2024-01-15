using System.Text;
using Azure.Identity;
using elastic_dotnet;
using elastic_dotnet.Config;
using elastic_dotnet.Data;
using elastic_dotnet.Models;
using elastic_dotnet.Repository;
using elastic_dotnet.Services;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

// AZURE CONFIG
var keyVaultUrl = new Uri(config.GetSection("KeyVaultUrl").Value!);
var azureCrendential = new DefaultAzureCredential();
builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCrendential);

// getting the keys from azure
var elasticCloudId = builder.Configuration.GetSection("elasticcloudid").Value!;
var elasticPassword = builder.Configuration.GetSection("elasticpassword").Value!;
var jwtSecret = builder.Configuration.GetSection("jwtsecret").Value!;

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = config.GetConnectionString("SqlServer");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddSingleton<IElasticClient>(services =>
{
    var elasticConfig = builder.Configuration.GetSection("Elastic").Get<ElasticConfiguration>() ?? throw new InvalidOperationException("Elasticsearch configuration is missing.");
    var settings = new ConnectionSettings(
        new CloudConnectionPool(elasticCloudId,
        new BasicAuthenticationCredentials(elasticConfig.Username, elasticPassword))
    )
    .DisableDirectStreaming()
    .DefaultMappingFor<Product>(m => m
        .PropertyName(p => p.ProductName, "ProductName")
        .PropertyName(p => p.Description, "Description")
    )
    .OnRequestCompleted(response =>
        {
            Console.WriteLine($"Request: {Encoding.UTF8.GetString(response.RequestBodyInBytes)}");
            Console.WriteLine($"Response: {Encoding.UTF8.GetString(response.ResponseBodyInBytes)}");
        });

    return new ElasticClient(settings);
});
builder.Services.AddScoped<ISentenceEncoder, SentenceEncoder>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IJwtService, JwtService>(options =>
{
    var jwtOptions = new JwtOptions();
    config.GetSection("JWT").Bind(jwtOptions);

    return new JwtService(new OptionsWrapper<JwtOptions>(jwtOptions), jwtSecret);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<PythonMicroserviceOptions>(builder.Configuration.GetSection("PythonMicroservice"));


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
