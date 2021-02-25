using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public abstract class DependencyContainerBase : IDependencyContainer
    {
        #region Constructors

        protected DependencyContainerBase()
        {
            Definitions = new DependencyDefinitions();
        }

        #endregion Constructors

        #region Properties

        protected IDependencyDefinitions Definitions { get; }

        #endregion Properties

        #region Methods

        public void Build(IList<IDependencyInfo> definitions)
        {
            if (definitions is null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            OnBuildStart();

            foreach (IDependencyInfo definition in definitions)
            {
                Definitions.Add(definition);
                OnBuild(definition);
            }

            OnBuildEnd();
        }

        public IDependencyInfo TryGetBinding<T>(object key)
        {
            return Definitions.TryGet<T>(key);
        }

        public IDependencyInfo TryGetBinding(Type requestType, object key)
        {
            return Definitions.TryGet(requestType, key);
        }

        protected abstract void OnBuild(IDependencyInfo definition);

        protected virtual void OnBuildEnd()
        { }

        protected virtual void OnBuildStart()
        { }

        #endregion Methods
    }
}