using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyTypeInfo : IDependencyInfo
    {
        Type TypeTo { get; }
    }
}