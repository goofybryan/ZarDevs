using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public static class ConfigureIoc
    {
        #region Methods

        public static IServiceCollection ConfigureIocBindings(this IServiceCollection services)
        {
            services.AddSingleton(p => Ioc.Container);

            return services;
        }

        #endregion Methods
    }
}