using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    internal interface IMultipleResolver<T>
    {
        IEnumerable<T> Resolved { get; }
    }

    internal class MultipleResolver<T> : IMultipleResolver<T>
    {
        public MultipleResolver(IEnumerable<T> resolved)
        {
            Resolved = resolved;
        }

        public IEnumerable<T> Resolved { get; }
    }
}