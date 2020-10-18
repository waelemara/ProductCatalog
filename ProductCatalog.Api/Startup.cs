using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductCatalog.Api.Domain.HttpClients;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Api
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
            services.AddTransient<GetSortedProductQueryHandler>();
            services.AddTransient<IProductHttpClient, ProductHttpClient>();
            services.AddTransient<IShopperHistoryHttpClient, ShopperHistoryHttpClient>();
            services.AddTransient<RecommendationsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/trolleyTotal", async context => { await ProxyToWoolisX(context); });

                endpoints.MapControllers();
            });
        }

        private static async Task ProxyToWoolisX(HttpContext context)
        {
            var proxyUrl = "http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/trolleyCalculator";
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var postRequestContent = await reader.ReadToEndAsync();
            var postJsonAsync = proxyUrl
                .SetQueryParam("token", "25a4f06f-8fd5-49b3-a711-c013c156f8c8")
                .WithHeader("Accept", "application/json")
                .WithHeader("Content-Type", "application/json-patch+json")
                .PostAsync(new StringContent(postRequestContent));

            var readAsStringAsync = await postJsonAsync.Result.Content.ReadAsStringAsync();

            await context.Response.WriteAsync(readAsStringAsync);
        }
    }
}