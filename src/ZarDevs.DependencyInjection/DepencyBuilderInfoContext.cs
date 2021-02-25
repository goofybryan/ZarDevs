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

        int ArgumentCount { get; }
        IIocContainer Ioc { get; }

        #endregion Properties

        #region Indexers

        object this[string name] { get; }

        #endregion Indexers

        #region Methods

        object[] GetArguments();

        void SetArguments(IList<(string, object)> namedArgs);

        void SetArguments(object[] args);

        #endregion Methods
    }

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

        public DepencyBuilderInfoContext(IIocContainer ioc, object[] args) : this(ioc)
        {
            SetArguments(args);
        }

        public DepencyBuilderInfoContext(IIocContainer ioc, (string, object)[] args) : this(ioc)
        {
            SetArguments(args);
        }

        #endregion Constructors

        #region Properties

        public int ArgumentCount => _arguments.Count;

        public IIocContainer Ioc { get; }

        #endregion Properties

        #region Indexers

        public object this[string name] => _arguments[name];

        #endregion Indexers

        #region Methods

        public object[] GetArguments() => _arguments.OrderBy(k => k.Key).Select(v => v.Value).ToArray();

        public void SetArguments(object[] args)
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

        public void SetArguments(IList<(string, object)> namedArgs)
        {
            if (namedArgs is null)
            {
                throw new ArgumentNullException(nameof(namedArgs));
            }

            _arguments.Clear();

            foreach (var (name, value) in namedArgs)
                AddArgument(name, value);
        }

        private void AddArgument(string name, object value)
        {
            _arguments[name] = value;
        }

        #endregion Methods
    }
}