
namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are configuration configurators that
    /// uses a <see cref="IConfigSource"/> instance as the container.
    /// </summary>
    public interface IConfigSourceConfigurator : IConfigurator<RegularConfigSource> { }

    /// <summary>
    /// Represents the base class for all configuration configurators that
    /// uses a <see cref="IConfigSource"/> instance as the container.
    /// </summary>
    public abstract class ConfigSourceConfigurator : Configurator<RegularConfigSource>, IConfigSourceConfigurator
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <c>ConfigSourceConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        public ConfigSourceConfigurator(IConfigSourceConfigurator context)
            : base(context) { }
        #endregion
    }
}
