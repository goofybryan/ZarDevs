using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <inheritdoc/>
    public class GenericTypeResolution : IGenericTypeResolution
    {
        private readonly ICreate _create;
        private readonly Type _genericFactoryType;

        /// <summary>
        /// Create a new instance of the <see cref="GenericTypeResolution"/>.
        /// </summary>
        /// <param name="create">Pass in the <see cref="ICreate"/> instance.</param>
        /// <param name="info">Pass in the info.</param>
        /// <param name="genericFactoryType">Pass in the <see cref="ITypeResolution"/> factory type</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public GenericTypeResolution(ICreate create, IDependencyInfo info, Type genericFactoryType)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            Info = info ?? throw new ArgumentNullException(nameof(info));

            if(genericFactoryType.GetInterface(nameof(ITypeResolution)) == null)
            {
                throw new ArgumentException($"The generic factory type is not an instance of typeof({typeof(ITypeResolution)})", nameof(genericFactoryType));
            }

            if(genericFactoryType.IsConstructedGenericType)
            {
                throw new ArgumentException($"The generic factory type '{genericFactoryType}' is already a constructed generic", nameof(genericFactoryType));
            }
                
            _genericFactoryType = genericFactoryType;
        }

        /// <inheritdoc/>
        public IDependencyInfo Info { get; }

        /// <inheritdoc/>
        public ITypeResolution MakeConcrete(Type type)
        {            
            var concreteInfo = Info.As(type.GenericTypeArguments);
            var concreteFactoryType = _genericFactoryType.MakeGenericType(type.GenericTypeArguments);

            return (ITypeResolution)_create.New(concreteFactoryType, concreteInfo);
        }

        /// <inheritdoc/>
        public object Resolve()
        {
            throw new InvalidOperationException("You cannot resolve a undefined generic resolution.");
        }

        /// <inheritdoc/>
        public object Resolve(params object[] parameters)
        {
            throw new InvalidOperationException("You cannot resolve a undefined generic resolution.");
        }

        /// <inheritdoc/>
        public object Resolve(params (string key, object value)[] parameters)
        {
            throw new InvalidOperationException("You cannot resolve a undefined generic resolution.");
        }
    }
}