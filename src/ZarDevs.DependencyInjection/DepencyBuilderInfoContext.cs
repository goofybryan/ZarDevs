using System;
using ZarDevs.Core;

namespace ZarDevs.DependencyInjection
{
    public class DepencyBuilderInfoContext
    {
        public DepencyBuilderInfoContext()
        {

        }

        public DepencyBuilderInfoContext(Type requestType, Type targetType)
        {
            RequestType = Check.IsNotNull(requestType, nameof(requestType));
            TargetType = Check.IsNotNull(targetType, nameof(targetType));
        }

        #region Properties

        public Type RequestType { get; set; }
        public Type TargetType { get; set; }

        #endregion Properties
    }
}