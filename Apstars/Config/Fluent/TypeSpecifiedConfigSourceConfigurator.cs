using System;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents the base class for the configuration configurators which configures
    /// the container with a specific type.
    /// </summary>
    public abstract class TypeSpecifiedConfigSourceConfigurator : ConfigSourceConfigurator
    {
        #region Private Fields
        private readonly Type type;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>TypeSpecifiedConfigSourceConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="type">The type that is needed by the configuration.</param>
        public TypeSpecifiedConfigSourceConfigurator(IConfigSourceConfigurator context, Type type)
            : base(context)
        {
            this.type = type;
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets the type that is needed by the configuration.
        /// </summary>
        protected Type Type
        {
            get { return type; }
        }
        #endregion
    }
}
