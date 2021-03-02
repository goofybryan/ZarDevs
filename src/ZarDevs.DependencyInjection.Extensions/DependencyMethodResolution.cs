namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency method resolution that will execute the method described in the <see cref="IDependencyMethodInfo"/>
    /// </summary>
    public class DependencyMethodResolution : DependencyResolution<IDependencyMethodInfo>
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the dependency method resolution.
        /// </summary>
        /// <param name="info">
        /// The the <see cref="IDependencyInstanceInfo"/> that describes this resolution.
        /// </param>
        public DependencyMethodResolution(IDependencyMethodInfo info) : base(info)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of ordered constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve(object[] args)
        {
            return Info.Execute(args);
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of named constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve((string, object)[] args)
        {
            return Info.Execute(args);
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve()
        {
            return Info.Execute();
        }

        #endregion Methods
    }
}