using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.DependencyInjection
{
    public static class ConfigureIoc
    {
        public static IDependencyBuilder ConfigureExtentions(this IDependencyBuilder builder)
        {
            builder.Bind<IDependencyResolver>().To<DependencyResolver>().InSingletonScope();

            return builder;
        }
    }
}
