using System;
using System.IO;
using System.Reflection;
using Ehex.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TariffComparisonApp.Data;
using TariffComparisonApp.Repositories;
using TariffComparisonApp.Repositories.Interfaces;

namespace TariffComparisonApp
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
            
            // swagger spec
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Tariff Comparison App", 
                    Version = "v1",
                    Description = "This API will <ul>" +
                                  "<li>Manage tariff products</li>" +
                                  "<li>Compare tariff products based on consumption value</li>" +
                                  "</ul>",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TariffComparison_MS_Database")), ServiceLifetime.Singleton);
            services.AddScoped<IProductRepository, ProductRepository>();
            
            // Global Exception Handler
            services.AddControllers(options => options.Filters.Add(new ApiExceptionHandlerFilter()));
        }
        
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
            // Setup Swagger Docs
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Tariff Comparison Application");
            });
            
        }
    }
}