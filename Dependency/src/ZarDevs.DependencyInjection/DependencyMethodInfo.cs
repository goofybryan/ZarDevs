using System;

namespace ZarDevs.DependencyInjection
{
    /// <inheritdoc/>
    public class DependencyMethodInfo : DependencyInfo, IDependencyMethodInfo
    {
        #region Constructors

        /// <inheritdoc/>
        public DependencyMethodInfo(Func<IDependencyContext, object> methodTo, IDependencyInfo info) : base(info)
        {
            Method = methodTo ?? throw new ArgumentNullException(nameof(methodTo));
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public Func<IDependencyContext, object> Method { get; }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public object Execute(IDependencyContext context)
        {
            return Method(context);
        }

        #endregion Methods
    }
}