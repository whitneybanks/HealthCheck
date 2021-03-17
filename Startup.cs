using HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Healthcheck
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
            /// ...existing code...
                services.AddControllersWithViews();
                // In production, the Angular files will be served
                // from this directory
                services.AddSpaStaticFiles(configuration =>
                {
                configuration.RootPath = "ClientApp/dist";
                });
                services.AddHealthChecks()
                .AddCheck("ICMP_01", new ICMPHealthCheck("www.ryadel.com", 100))
                .AddCheck("ICMP_02", new ICMPHealthCheck("www.google.com", 100))
                .AddCheck("ICMP_03", new ICMPHealthCheck("www.does-not-exist.com", 100));        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /// ...existing code...
            
            app.UseRouting();

            app.UseHealthChecks("/hc", new CustomHealthCheckOptions());

            app.UseEndpoints (endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            /// ...existing code...
        }
    }
}
