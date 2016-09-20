using System;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are identity generator configurators.
    /// </summary>
    public interface IIdentityGeneratorConfigurator : IConfigSourceConfigurator
    { }

    /// <summary>
    /// Represents the identity generator configurator.
    /// </summary>
    public class IdentityGeneratorConfigurator : TypeSpecifiedConfigSourceConfigurator, IIdentityGeneratorConfigurator
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>IdentityGeneratorConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="identityGeneratorType">The type of the generator to be used in the application.</param>
        public IdentityGeneratorConfigurator(IConfigSourceConfigurator context, Type identityGeneratorType)
            : base(context, identityGeneratorType)
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
            container.IdentityGenerator = Type;
            return container;
        }
        #endregion
    }
}
