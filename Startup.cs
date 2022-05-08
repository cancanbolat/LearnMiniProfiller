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
using Microsoft.OpenApi.Models;
using StackExchange.Profiling.Storage;

namespace miniProfiller
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

            services.AddMemoryCache();

            services.AddMiniProfiler(op =>
            {
                op.RouteBasePath = "/profiler";

                //60 dakikada bir depolama kontrol edilecektir.
                //Varsayılan değeri 30 dakikadır.                
                (op.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                //SQL formatlayıcısı kontrol edilmektedir.
                //Varsayılan olarak InlineFormatter'dir.
                op.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                //SQL formatlayıcısı kontrol edilmektedir.
                //Varsayılan olarak InlineFormatter'dir.
                op.TrackConnectionOpenClose = true;
            }).AddEntityFramework();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "miniProfiller", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "miniProfiller v1"));
                app.UseMiniProfiler();
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
