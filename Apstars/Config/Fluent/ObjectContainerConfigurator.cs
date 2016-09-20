using System;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are object container configurators.
    /// </summary>
    public interface IObjectContainerConfigurator : IConfigSourceConfigurator
    { }

    /// <summary>
    /// Represents the object container configurator.
    /// </summary>
    public class ObjectContainerConfigurator : TypeSpecifiedConfigSourceConfigurator, IObjectContainerConfigurator
    {
        private readonly bool initFromConfigFile = false;
        private readonly string sectionName = null;
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ObjectContainerConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="objectContainerType">The type of the object container to be used by the application.</param>
        /// <param name="initFromConfigFile">The <see cref="Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        public ObjectContainerConfigurator(IConfigSourceConfigurator context, Type objectContainerType, bool initFromConfigFile, string sectionName)
            : base(context, objectContainerType)
        {
            this.initFromConfigFile = initFromConfigFile;
            this.sectionName = sectionName;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The configuration container.</param>
        /// <returns>The configured container.</returns>
        protected override RegularConfigSource DoConfigure(RegularConfigSource container)
        {
            container.ObjectContainer = Type;
            container.InitObjectContainerFromConfigFile = this.initFromConfigFile;
            container.ObjectContainerSectionName = this.sectionName;
            return container;
        }
        #endregion
    }
}
