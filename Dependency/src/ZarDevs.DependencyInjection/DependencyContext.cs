using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// The dependency information context class used for custom IOC request specifically for requests
    /// </summary>
    public interface IDependencyContext
    {
        #region Properties

        /// <summary>
        /// Get the argument count
        /// </summary>
        int ArgumentCount { get; }

        /// <summary>
        /// Get the dependency information that resulted in the context being created.
        /// </summary>
        IDependencyInfo Info { get; }

        /// <summary>
        /// Get the IOC constainer
        /// </summary>
        IIocContainer Ioc { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the arguments associated with this context
        /// </summary>
        /// <returns></returns>
        object[] GetArguments();

        /// <summary>
        /// Set the arguments for the context.
        /// </summary>
        /// <param name="args">The context arguments</param>
        IDependencyContext SetArguments(object[] args);

        /// <summary>
        /// Set the arguments for the context.
        /// </summary>
        /// <param name="namedArgs">The context arguments</param>
        IDependencyContext SetArguments(IList<(string, object)> namedArgs);

        #endregion Methods
    }

    /// <summary>
    /// Dependency information context used for resolving instances.
    /// </summary>
    public class DependencyContext : IDependencyContext
    {
        #region Fields

        private readonly IDictionary<string, object> _arguments;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the context class.
        /// </summary>
        /// <param name="ioc">Specify the IOC container</param>
        /// <param name="info">Specify the binding info</param>
        public DependencyContext(IIocContainer ioc, IDependencyInfo info)
        {
            Ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
            Info = info ?? throw new ArgumentNullException(nameof(info));
            _arguments = new Dictionary<string, object>();
        }

        /// <summary>
        /// Create a new instance of the context class.
        /// </summary>
        /// <param name="ioc">Specify the IOC container.</param>
        /// <param name="info">Specify the binding info</param>
        /// <param name="args">A list of ordered args.</param>
        public DependencyContext(IIocContainer ioc, IDependencyInfo info, object[] args) : this(ioc, info)
        {
            SetArguments(args);
        }

        /// <summary>
        /// Create a new instance of the context class.
        /// </summary>
        /// <param name="ioc">Specify the IOC container</param>
        /// <param name="info">Specify the binding info</param>
        /// <param name="args">A list of named arguments.</param>
        public DependencyContext(IIocContainer ioc, IDependencyInfo info, (string, object)[] args) : this(ioc, info)
        {
            SetArguments(args);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the argument count
        /// </summary>
        public int ArgumentCount => _arguments.Count;

        /// <summary>
        /// Get the dependency information that resulted in the context being created.
        /// </summary>
        public IDependencyInfo Info { get; }

        /// <summary>
        /// Get the IOC constainer
        /// </summary>
        public IIocContainer Ioc { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the arguments associated with this context
        /// </summary>
        /// <returns></returns>
        public object[] GetArguments() => _arguments.OrderBy(k => k.Key).Select(v => v.Value).ToArray();

        /// <summary>
        /// Set the arguments for the context.
        /// </summary>
        /// <param name="args">The context arguments</param>
        public IDependencyContext SetArguments(object[] args)
        {
            if (args is null)
                return this;

            _arguments.Clear();

            for (int i = 0; i < args.Length; i++)
            {
                AddArgument(i.ToString(), args[i]);
            }

            return this;
        }

        /// <summary>
        /// Set the arguments for the context.
        /// </summary>
        /// <param name="namedArgs">The context arguments</param>
        public IDependencyContext SetArguments(IList<(string, object)> namedArgs)
        {
            if (namedArgs is null)
                return this;

            _arguments.Clear();

            foreach (var (name, value) in namedArgs)
                AddArgument(name, value);

            return this;
        }

        private void AddArgument(string name, object value)
        {
            _arguments[name] = value;
        }

        #endregion Methods
    }
}