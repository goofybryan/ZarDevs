using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// The dependency builder information context class used for custom IOC request specifically
    /// for requests
    /// </summary>
    public interface IDepencyBuilderInfoContext
    {
        #region Properties

        /// <summary>
        /// Get the argument count
        /// </summary>
        int ArgumentCount { get; }

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

        #endregion Methods
    }

    /// <summary>
    /// Dependency builder information context used for resolving instances.
    /// </summary>
    public class DepencyBuilderInfoContext : IDepencyBuilderInfoContext
    {
        #region Fields

        private readonly IDictionary<string, object> _arguments;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the context class.
        /// </summary>
        /// <param name="ioc">Specify the IOC container</param>
        public DepencyBuilderInfoContext(IIocContainer ioc)
        {
            Ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
            _arguments = new Dictionary<string, object>();
        }

        /// <summary>
        /// Create a new instance of the context class.
        /// </summary>
        /// <param name="ioc">Specify the IOC container.</param>
        /// <param name="args">A list of ordered args.</param>
        public DepencyBuilderInfoContext(IIocContainer ioc, object[] args) : this(ioc)
        {
            SetArguments(args);
        }

        /// <summary>
        /// Create a new instance of the context class.
        /// </summary>
        /// <param name="ioc">Specify the IOC container</param>
        /// <param name="args">A list of named arguments.</param>
        public DepencyBuilderInfoContext(IIocContainer ioc, (string, object)[] args) : this(ioc)
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

        private void AddArgument(string name, object value)
        {
            _arguments[name] = value;
        }

        private void SetArguments(object[] args)
        {
            if (args is null)
            {
                return;
            }

            _arguments.Clear();

            for (int i = 0; i < args.Length; i++)
            {
                AddArgument(i.ToString(), args[i]);
            }
        }

        private void SetArguments(IList<(string, object)> namedArgs)
        {
            if (namedArgs is null)
            {
                throw new ArgumentNullException(nameof(namedArgs));
            }

            _arguments.Clear();

            foreach (var (name, value) in namedArgs)
                AddArgument(name, value);
        }

        #endregion Methods
    }
}