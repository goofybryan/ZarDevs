using System;

namespace ZarDevs.DependencyInjection
{
    public class DepencyBuilderInfoContext
    {
        #region Constructors

        public DepencyBuilderInfoContext()
        {
        }

        public DepencyBuilderInfoContext(Type requestType, Type targetType)
        {
            RequestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        #endregion Constructors

        #region Properties

        public Type RequestType { get; set; }
        public Type TargetType { get; set; }

        #endregion Properties
    }
}