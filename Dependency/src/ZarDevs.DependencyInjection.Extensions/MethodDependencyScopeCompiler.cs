﻿using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Compiler for <see cref="IDependencyMethodInfo"/>
    /// </summary>
    public class MethodDependencyScopeCompiler : DependencyScopeBinderBase<IDependencyResolutionConfiguration, IDependencyMethodInfo>
    {
        private readonly IDependencyResolutionFactory _resolutionFactory;

        /// <summary>
        /// Create a new instance of the <see cref="TypedDependencyScopeCompiler"/>
        /// </summary>
        /// <param name="resolutionFactory">An instance of the resolution factory.</param>
        /// <param name="scopes">Specify the scopes that this is valid for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolutionFactory"/> is null.</exception>
        public MethodDependencyScopeCompiler(IDependencyResolutionFactory resolutionFactory, DependyBuilderScopes scopes) : base(scopes)
        {
            _resolutionFactory = resolutionFactory ?? throw new ArgumentNullException(nameof(resolutionFactory));
        }

        /// <inheritdoc/>
        protected override void OnRegisterTransient(IDependencyResolutionConfiguration container, IDependencyMethodInfo info)
        {
            container.Add(info.ResolvedTypes, _resolutionFactory.ResolutionFor(info));
        }

        /// <inheritdoc/>
        protected override void OnRegisterSingleton(IDependencyResolutionConfiguration container, IDependencyMethodInfo info)
        {
            container.Add(info.ResolvedTypes, new DependencySingletionResolution<IDependencyMethodInfo, IDependencyResolution<IDependencyMethodInfo>>(_resolutionFactory.ResolutionFor(info)));
        }
    }
}