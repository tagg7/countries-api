using Countries.Application.Interfaces;
using Countries.Application.Queries;
using Countries.Domain.Common;
using Countries.Domain.Entities;
using Countries.Infrastructure;
using Countries.Web.MappingProfiles;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using StackExchange.Redis;

namespace Countries.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Countries.Web", Version = "v1" });
            });
            
            // TODO: Move to separate registration class(es)
            services.AddAutoMapper(config => config.AddMaps(typeof(CountryMappingProfile).Assembly));
            services.AddMediatR(typeof(GetCountryQueryHandler).Assembly);

            var mongoUrl = MongoUrl.Create(Configuration.GetValue<string>("MongoDb"));
            services.AddSingleton(new MongoClient(mongoUrl.Url).GetDatabase(mongoUrl.DatabaseName));
            
            BsonClassMap.RegisterClassMap<EntityBase>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIgnoreIfDefault(true);
            });
            
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    EndPoints = { Configuration.GetValue<string>("Redis") }
                }));

            services.AddSingleton<ICountryService, CountryService>();
            services.AddSingleton<IDbRepository<Country>, MongoDbRepository>();
            services.AddSingleton<ICache<Country>, RedisCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Countries.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}