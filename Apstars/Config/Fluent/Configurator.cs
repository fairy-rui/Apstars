using System.ComponentModel;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are configuration configurators.
    /// </summary>
    /// <typeparam name="TContainer">The type of the object container.</typeparam>
    public interface IConfigurator<TContainer>
    {
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <returns>The configured container.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        TContainer Configure();
    }

    /// <summary>
    /// Represents the base class for all the configuration configurators.
    /// </summary>
    /// <typeparam name="TContainer">The type of the object container.</typeparam>
    public abstract class Configurator<TContainer> : IConfigurator<TContainer>
    {
        #region Private Fields
        private readonly IConfigurator<TContainer> context;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>Configurator{TContainer}</c> class.
        /// </summary>
        /// <param name="context">The <see cref="IConfigurator{TContainer}"/> instance.</param>
        /// <remarks>The <paramref name="context"/> parameter specifies the configuration context
        /// which was provided by the previous configuration step and will be configured in the
        /// current step.</remarks>
        public Configurator(IConfigurator<TContainer> context)
        {
            this.context = context;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the configuration context instance.
        /// </summary>
        public IConfigurator<TContainer> Context
        {
            get { return this.context; }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The object container to be configured.</param>
        /// <returns>The configured container.</returns>
        protected abstract TContainer DoConfigure(TContainer container);
        #endregion

        #region IConfigurator<TContainer> Members
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <returns>The configured container.</returns>
        public TContainer Configure()
        {
            var container = this.context.Configure();
            return DoConfigure(container);
        }

        #endregion
    }
}
