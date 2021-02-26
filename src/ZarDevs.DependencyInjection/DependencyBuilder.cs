using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency builder used to build up the dependencies that will be translated to an
    /// appropriate IOC solution.
    /// </summary>
    public class DependencyBuilder : IDependencyBuilder
    {
        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="container"></param>
        public DependencyBuilder(IDependencyContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #endregion Constructors

        #region Properties

        private IDependencyContainer Container { get; }
        private IList<IDependencyBuilderInfo> Definitions { get; } = new List<IDependencyBuilderInfo>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <param name="type">The specified type to bind.</param>
        /// <returns></returns>
        public IDependencyBuilderBindingResolve Bind(Type type)
        {
            var info = new DependencyBuilderInfo();
            Definitions.Add(info);
            return info.Bind(type);
        }

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type to bind.</typeparam>
        /// <returns></returns>
        public IDependencyBuilderBindingResolve Bind<T>() where T : class
        {
            return Bind(typeof(T));
        }

        /// <summary>
        /// Build the dependencies.
        /// </summary>
        public void Build() => Container.Build(Definitions.Select(definition => definition.DependencyInfo).ToList());

        #endregion Methods
    }
}