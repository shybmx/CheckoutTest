using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.API.Processes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Checkout.Payment.Gateway.Contracts;
using System;
using Microsoft.Extensions.Logging;
using Polly;

namespace Checkout.Payment.Gateway.API
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
            Install(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Checkout Payment Gatway", Version = "v1" });
            });
        }

        private void Install(IServiceCollection services)
        {
            InstallHttpWrapper(services);
            InstallPaymentProcess(services);
            InstallCosmos(services);
        }

        private void InstallCosmos(IServiceCollection services)
        {
            var cosmosConfiguration = new CosmosConfiguration();
            Configuration.GetSection("Cosmos").Bind(cosmosConfiguration);

            services.AddSingleton<ICosmosDatabaseWrapper<SavedPaymentDetails>, CosmosDatabaseWrapper<SavedPaymentDetails>>(c => new CosmosDatabaseWrapper<SavedPaymentDetails>(cosmosConfiguration));
            services.AddSingleton<ICosmosDatabaseClient, CosmosDatabaseClient>();
        }

        private void InstallHttpWrapper(IServiceCollection services)
        {
            services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
        }

        private void InstallPaymentProcess(IServiceCollection services)
        {
            var paymentConfiguration = new PaymentConfiguration();
            Configuration.GetSection("Payment").Bind(paymentConfiguration);

            services.AddSingleton<IPaymentProcess, PaymentProcess>(c => new PaymentProcess(paymentConfiguration, new HttpClientWrapper()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout Payment Gatway"));

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
