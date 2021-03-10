using System;
using System.Collections;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    internal interface IMultipleResolver<T>
    {
        #region Properties

        IEnumerable<T> Resolved { get; }

        #endregion Properties
    }

    internal class MultipleResolver<T> : MultipleResolver, IMultipleResolver<T>
    {
        #region Constructors

        public MultipleResolver(IEnumerable<T> resolved) : base(resolved)
        {
            Resolved = resolved;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<T> Resolved { get; }

        #endregion Properties
    }

    internal class MultipleResolver
    {
        #region Constructors

        public MultipleResolver(IEnumerable resolvedAsObject)
        {
            ResolvedAsObject = resolvedAsObject;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable ResolvedAsObject { get; }

        #endregion Properties

        #region Methods

        public static Type MakeConcreateOfType(Type type)
        {
            Type generic = typeof(IMultipleResolver<>);

            return generic.MakeGenericType(type);
        }

        #endregion Methods
    }
}