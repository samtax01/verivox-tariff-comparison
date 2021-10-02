using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TariffComparisonApp.Data;

#pragma warning disable 1570
namespace TariffComparisonApp.Tests.Helpers
{
    
    public abstract class AbstractIntegrationTest: IDisposable
    {
        
        /// <summary>
        /// Local InMemory Database Context
        /// </summary>
        protected readonly DatabaseContext DatabaseContext;
        
        /// <summary>
        /// InMemory Database Context to Access TestServer Context.
        /// </summary>
        protected readonly DatabaseContext TestServerDatabaseContext;


        /// <summary>
        /// Sample usage
        ///     var response = await TestServer.PostAsJsonAsync("/api/v1/...", "..." )
        ///     var data = response.Content.ReadAsStringAsync();
        /// 
        /// Or Using HttpRequest Helper
        ///     var httpResponse = await HttpRequest.MakeAsync(TestServer, "/api/v1/...", HttpMethod.Post);
        ///     var apiResponse = await ApiResponse<SampleModel>.FromRequestAsync(httpResponse);
        /// </summary>
        protected readonly HttpClient TestServer;
        //protected readonly HttpClient TestServerForValidation;
        
        /// <summary>
        /// For Dependency Injection to get App Services
        /// e.g
        ///  var databaseContext = TestServerServiceProvider.GetRequiredService<DatabaseContext>()
        ///  var logger = TestServerServiceProvider.GetService<ILogger<AbstractIntegrationTest>>() ?? NullLogger<AbstractIntegrationTest>.Instance;
        /// </summary>
        protected readonly IServiceProvider TestServerServiceProvider;
        
        
        
        protected AbstractIntegrationTest()
        {
            
            // Create Database
            var databaseContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            DatabaseContext = new DatabaseContext(databaseContextOptions);
            DatabaseContext.Database.EnsureCreated();

            
            static void InMemoryConfiguration(IServiceCollection services)
            {
                // Convert Real Database to InMemory Database
                services.RemoveAll(typeof(DatabaseContext));
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase("TestServerDB"); //(Guid.NewGuid().ToString());
                });
            }
            static void AppSettingsConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();
            }
            
            // HttpClient 1
            var webHostBuilder = new WebHostBuilder().UseEnvironment("development").UseStartup<Startup>().ConfigureServices(InMemoryConfiguration).ConfigureAppConfiguration(AppSettingsConfiguration);
            
            // Init TestServer Services
            var server = new TestServer(webHostBuilder);
            TestServer = server.CreateClient(); //// TODO: Initially not working with all validations.
            TestServer.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            TestServerServiceProvider = server.Host.Services.CreateScope().ServiceProvider;
            TestServerDatabaseContext = TestServerServiceProvider.GetRequiredService<DatabaseContext>();
        }


        protected async Task AuthenticateAsync()
        {
            var auth = new AuthenticationHeaderValue("bearer", await GetJWTAsync());
            //TestServerForValidation.DefaultRequestHeaders.Authorization = auth;
            TestServer.DefaultRequestHeaders.Authorization = auth;
        }

        private async Task<string> GetJWTAsync()
        {
            // Create a user to get token
            // var response = await _httpClient.PostAsJsonAsync("...url...", new UserRequest { });
            // var registrationResponse = await response.Content.ReadFromJsonAsync<ApiResponse<User>>();
            // return registrationResponse.token; 
            return await Task.FromResult("");
        }
        
        
        
        public void Dispose()
        {
            DatabaseContext.Database.EnsureDeleted();
            DatabaseContext.Dispose();
            TestServer.Dispose();
            //TestServerForValidation.Dispose();
        }
    }
}