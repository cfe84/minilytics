using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureDataStore;
using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Minilytics
{
    public class Startup
    {
        static string STORAGE_CONNECTION_STRING = Environment.GetEnvironmentVariable("STORAGE_CONNECTION_STRING");
        static string EVENTS_TABLE_NAME = Environment.GetEnvironmentVariable("TABLE_NAME") ?? "pageviews";
        static string EXCEPTION_TABLE_NAME = "exceptions";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSingleton<IEventStore>(
                new AzureEventStore(
                    STORAGE_CONNECTION_STRING, 
                    EVENTS_TABLE_NAME,
                    EXCEPTION_TABLE_NAME)
                    );
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
