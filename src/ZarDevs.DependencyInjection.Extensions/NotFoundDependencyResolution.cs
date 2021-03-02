using System;

namespace ZarDevs.DependencyInjection
{
    internal class NotFoundDependencyResolution : IDependencyResolution
    {
        private readonly Type _requestType;
        private readonly object _key;
        private readonly bool _throwError;

        public NotFoundDependencyResolution(Type requestType, object key, bool throwError)
        {
            _requestType = requestType;
            _key = key;
            _throwError = throwError;
        }

        public NotFoundDependencyResolution() : this(null, null, false)
        {

        }

        public object Key => null;

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
    }
}