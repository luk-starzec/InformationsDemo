using ApiHelpers;
using HealthChecks.UI.Client;
using InformationsApi.Repo;
using InformationsApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;

namespace InformationsApi
{
    public class Startup
    {
        readonly string SwaggerApiName = "Informations API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var seasonsApiUrl = Environment.GetEnvironmentVariable("SeasonsApiUrl");
            services
                .AddSingleton<ISeasonsService, SeasonsService>(s => new SeasonsService(seasonsApiUrl))
                .AddSingleton<IInformationsRepo, InformationsRepo>()
                .AddSingleton<IInformationsService, InformationsService>();

            ConfigureLogger();

            services
                .AddCustomVersioning()
                .AddCustomSwagger(SwaggerApiName)
                .AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            loggerFactory.AddSerilog();

            app.UseCustomSwagger(SwaggerApiName, provider);

            app.UseHealthChecks("/hc", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
        }


        private void ConfigureLogger()
        {
            var elasticUrl = Environment.GetEnvironmentVariable("ElasticUrl");
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl)) { AutoRegisterTemplate = true, })
                .CreateLogger();
        }
    }
}
