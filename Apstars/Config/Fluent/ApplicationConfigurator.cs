using System;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are application configurators.
    /// </summary>
    public interface IApplicationConfigurator : IConfigSourceConfigurator
    { }

    /// <summary>
    /// Represents the application configurator.
    /// </summary>
    public class ApplicationConfigurator : TypeSpecifiedConfigSourceConfigurator, IApplicationConfigurator
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ApplicationConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="appType">The type of the application.</param>
        public ApplicationConfigurator(IConfigSourceConfigurator context, Type appType)
            : base(context, appType)
        { }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The configuration container.</param>
        /// <returns>The configured container.</returns>
        protected override RegularConfigSource DoConfigure(RegularConfigSource container)
        {
            container.Application = Type;
            return container;
        }
        #endregion
    }
}
