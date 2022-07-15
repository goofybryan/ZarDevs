using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Base dependency info class that describes the <see cref="ResolvedTypes"/>, <see cref="Scope"/>
    /// and optionally <see cref="Key"/> for any binding.
    /// </summary>
    public abstract class DependencyInfo : IDependencyInfo
    {
        #region Constructors

        /// <summary>
        /// Create a new instance with the base variables set.
        /// </summary>
        /// <param name="key">Specify the key, optional.</param>
        /// <param name="scope">Specify the scope, optional, default is <see cref="DependyBuilderScopes.Transient"/></param>
        protected DependencyInfo(object key = null, DependyBuilderScopes scope = DependyBuilderScopes.Transient)
        {
            ResolvedTypes = new HashSet<Type>();
            Key = key;
            Scope = scope;
        }

        /// <summary>
        /// Create a new instance with copied values.
        /// </summary>
        /// <param name="copy">The base instance to copy from.</param>
        protected DependencyInfo(IDependencyInfo copy) : this()
        {
            if (copy == null) return;

            Key = copy.Key;
            Scope = copy.Scope;

            foreach (var resolvedType in copy.ResolvedTypes)
            {
                ResolvedTypes.Add(resolvedType);
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// An optional key that will be used to define the binding.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Specify the resolved types for this binding.
        /// </summary>
        public ISet<Type> ResolvedTypes { get; }

        /// <summary>
        /// The scope that the binding will be defined to, default is <see cref="DependyBuilderScopes.Transient"/>
        /// </summary>
        public DependyBuilderScopes Scope { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a new instance of the dependency context.
        /// </summary>
        /// <returns></returns>
        public IDependencyContext CreateContext(IIocContainer ioc)
        {
            return new DependencyContext(ioc, this);
        }

        /// <summary>
        /// Overridden ToString describing the class properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Dependency Info: Key={Key}, Scope={Scope}, ResolveTypes={ResolvedTypes.ToArray()}";
        }

        #endregion Methods
    }
}