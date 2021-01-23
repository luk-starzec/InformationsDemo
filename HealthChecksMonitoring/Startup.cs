using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HealthChecksMonitoring
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

            var json = Environment.GetEnvironmentVariable("Endpoints");
            var endpoints = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            services
                .AddHealthChecksUI(setup =>
                {
                    foreach (var endpoint in endpoints)
                        setup.AddHealthCheckEndpoint(endpoint.Key, endpoint.Value);
                })
                .AddInMemoryStorage();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(config =>
            {
                config.MapHealthChecksUI(c => c.UIPath = "/");
            });
        }
    }
}
