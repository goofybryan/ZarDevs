using System;
using System.Collections;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    internal interface IMultipleResolver<T>
    {
        IEnumerable<T> Resolved { get; }
    }

    internal class MultipleResolver<T> : MultipleResolver, IMultipleResolver<T>
    {
        public MultipleResolver(IEnumerable<T> resolved) : base(resolved)
        {
            Resolved = resolved;
        }

        public IEnumerable<T> Resolved { get; }
    }

    internal class MultipleResolver
    {
        public MultipleResolver(IEnumerable resolvedAsObject)
        {
            ResolvedAsObject = resolvedAsObject;
        }

        public IEnumerable ResolvedAsObject { get; }

        public static Type MakeConcreateOfType(Type type)
        {
            Type generic = typeof(IMultipleResolver<>);

            return generic.MakeGenericType(type);
        }

    }
}