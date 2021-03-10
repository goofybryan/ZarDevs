using System;

namespace ZarDevs.DependencyInjection
{
    internal class NotFoundDependencyResolution : IDependencyResolution
    {
        #region Fields

        private readonly object _key;
        private readonly Type _requestType;
        private readonly bool _throwError;

        #endregion Fields

        #region Constructors

        public NotFoundDependencyResolution(Type requestType, object key, bool throwError)
        {
            _requestType = requestType;
            _key = key;
            _throwError = throwError;
        }

        public NotFoundDependencyResolution() : this(null, null, false)
        {
        }

        #endregion Constructors

        #region Properties

        public object Key => null;

        #endregion Properties

        #region Methods

        public IDependencyResolution MakeConcrete(Type concreteRequest)
        {
            throw new NotSupportedException("This is not supported by the not found resolution");
        }

        public object Resolve()
        {
            return !_throwError ? null : throw new DependencyResolutionNotConfiguredException(_requestType, _key);
        }

        public object Resolve(object[] args)
        {
            return Resolve();
        }

        public object Resolve((string, object)[] args)
        {
            return Resolve();
        }

        public object Resolve(IDependencyContext context)
        {
            return Resolve();
        }

        #endregion Methods
    }
}