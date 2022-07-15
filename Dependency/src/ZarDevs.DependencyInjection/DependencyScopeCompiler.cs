using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <inheritdoc/>
    public class DependencyScopeCompiler<TContainer> : IDependencyScopeCompiler<TContainer> where TContainer : class
    {
        private readonly IDependencyScopeBinder<TContainer>[] _scopeBinders;

        /// <summary>
        /// Create a new instance of the DependencyScopeCompiler
        /// </summary>
        /// <param name="scopeBinders">A list of scope binders.</param>
        /// <exception cref="System.ArgumentNullException">When <paramref name="scopeBinders"/> is null</exception>
        /// <exception cref="System.ArgumentException">When <paramref name="scopeBinders"/> is empty</exception>
        public DependencyScopeCompiler(params IDependencyScopeBinder<TContainer>[] scopeBinders)
        {
            _scopeBinders = scopeBinders?.ToArray() ?? throw new System.ArgumentNullException(nameof(scopeBinders));

            if (_scopeBinders.Length == 0) throw new System.ArgumentException("No scope binders have been defined", nameof(scopeBinders));
        }

        /// <summary>
        /// Create a new instance of the DependencyScopeCompiler
        /// </summary>
        /// <param name="scopeBinders">A list of scope binders.</param>
        /// <exception cref="System.ArgumentNullException">When <paramref name="scopeBinders"/> is null</exception>
        /// <exception cref="System.ArgumentException">When <paramref name="scopeBinders"/> is empty</exception>
        public DependencyScopeCompiler(IEnumerable<IDependencyScopeBinder<TContainer>> scopeBinders) : this(scopeBinders?.ToArray())
        {
        }

        /// <inheritdoc/>
        public IDependencyScopeBinder<TContainer> FindBinder(IDependencyInfo definition)
        {
            return _scopeBinders.FirstOrDefault(binder => binder.CanBind(definition));
        }
    }
}