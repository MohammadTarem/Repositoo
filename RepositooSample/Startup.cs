using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace RepositooSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            // For Sql
            services.AddDbContext<OrdersContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RepositooSample"));
            });

            // For MongoDB
            //var s = Configuration.GetConnectionString("MongoDb");
            //services.AddSingleton<IMongoClient>(new MongoClient(Configuration.GetConnectionString("MongoDb")));

            

            BsonClassMap.RegisterClassMap<Order>(o =>
            {
                o.AutoMap();
                o.SetIgnoreExtraElements(true);

            });

        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OrdersContext context)//, IMongoClient db)
        {

            // Create database

            // For sql db
            context.Database.EnsureCreated();

            // For mongo db
            //db.DropDatabase("Orders");





            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
