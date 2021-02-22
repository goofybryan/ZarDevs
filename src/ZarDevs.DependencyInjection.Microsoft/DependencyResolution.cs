using System;

namespace ZarDevs.DependencyInjection
{
    public abstract class DependencyResolution<TInfo> : IDependencyResolution<TInfo> where TInfo : IDependencyInfo
    {
        public DependencyResolution(TInfo info)
        {
            if (info is null) throw new ArgumentNullException("Info cannot be null", nameof(info));

            Info = info;
        }

        public object Key => Info.Key ?? string.Empty;

        public TInfo Info { get; }

        public abstract object Resolve(IIocContainer ioc, params object[] args);

        public abstract object Resolve(IIocContainer ioc, params (string, object)[] args);
    }
}