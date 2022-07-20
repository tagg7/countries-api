using Countries.Application.Interfaces;
using Countries.Domain.Entities;
using Countries.Infrastructure;
using Countries.Web.MappingProfiles;
using MongoDB.Driver;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(config => config.AddMaps(typeof(CountryMappingProfile).Assembly));

// TODO: Move to separate registration class(es)
builder.Services.AddSingleton(new MongoClient(builder.Configuration.GetValue<string>("MongoDb")));
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(new ConfigurationOptions
    {
        EndPoints = { builder.Configuration.GetValue<string>("Redis") },
        AbortOnConnectFail = false
    }));

builder.Services.AddSingleton<ICountryService, CountryService>();
builder.Services.AddSingleton<IDbRepository<Country>, MongoDbRepository>();
builder.Services.AddSingleton<ICache<Country>, RedisCache>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();