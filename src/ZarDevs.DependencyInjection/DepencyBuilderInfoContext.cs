using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// The dependency builder information context class used for custom IOC request specifically for requests
    /// </summary>
    public class DepencyBuilderInfoContext
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the context class.
        /// </summary>
        /// <param name="ioc">Specify the IOC container</param>
        /// <param name="targetType">Specify the Target Type the request is for.</param>
        public DepencyBuilderInfoContext(IIocContainer ioc, Type targetType)
        {
            Ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// The target type that this request is for
        /// </summary>
        public Type TargetType { get; set; }

        /// <summary>
        /// Get the IOC container.
        /// </summary>
        public IIocContainer Ioc { get; }

        #endregion Properties
    }
}