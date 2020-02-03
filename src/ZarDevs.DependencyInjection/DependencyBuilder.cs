using System;
using System.Collections.Generic;
using System.Linq;
using ZarDevs.Core;

namespace ZarDevs.DependencyInjection
{
    public class DependencyBuilder : IDependencyBuilder
    {
        #region Constructors

        public DependencyBuilder(IDependencyContainer container)
        {
            Container = Check.IsNotNull(container, nameof(container));
        }

        #endregion Constructors

        #region Properties

        public IDependencyContainer Container { get; }
        public IList<IDependencyBuilderInfo> Definitions { get; } = new List<IDependencyBuilderInfo>();

        #endregion Properties

        #region Methods

        public IDependencyBuilderInfo Bind(Type type)
        {
            var info = new DependencyBuilderInfo().Bind(type);
            Definitions.Add(info);
            return info;
        }

        public IDependencyBuilderInfo Bind<T>() where T : class
        {
            return Bind(typeof(T));
        }

        public void Build() => Container.Build(Definitions.Select(definition => definition.DependencyInfo).ToList());

        #endregion Methods
    }
}