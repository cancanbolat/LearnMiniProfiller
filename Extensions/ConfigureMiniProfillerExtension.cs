using System;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Profiling.Storage;

namespace miniProfiller.Extensions
{
    public static class ConfigureMiniProfillerExtension
    {
        public static IServiceCollection MiniProfiler(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddMiniProfiler(op =>
            {
                op.RouteBasePath = "/profiller";

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

            return services;
        }
    }
}
