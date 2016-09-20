using System;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are sequence generator configurators.
    /// </summary>
    public interface ISequenceGeneratorConfigurator : IConfigSourceConfigurator
    { }

    /// <summary>
    /// Represents the sequence generator configurator.
    /// </summary>
    public class SequenceGeneratorConfigurator : TypeSpecifiedConfigSourceConfigurator, ISequenceGeneratorConfigurator
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>SequenceGeneratorConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="sequenceGeneratorType">The type of the generator to be used in the application.</param>
        public SequenceGeneratorConfigurator(IConfigSourceConfigurator context, Type sequenceGeneratorType)
            : base(context, sequenceGeneratorType)
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
            container.SequenceGenerator = Type;
            return container;
        }
        #endregion
    }
}
