using Amaze.Coin.Api;
using Amaze.Coin.Api.Services;
using Amaze.Coin.Api.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amaze.Coin
{
    public class Startup
    {
        private static IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", true, true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var appSettings = Configuration.GetSection("App").Get<AppSettings>();
            var cipherService = new CipherService(DataProtectionProvider.Create("amaze-coin"));
            
            var adminStore = new AdminStore(appSettings, cipherService);
            var accountStore = new AccountStore(appSettings, adminStore);

            services.AddSingleton<ICipherService>(cipherService);
            services.AddSingleton<IAdminStore>(adminStore);
            services.AddSingleton<IAccountStore>(accountStore);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
