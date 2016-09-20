
namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are Apstars configurators which configures
    /// the Apstars framework.
    /// </summary>
    public interface IApstarsConfigurator : IConfigSourceConfigurator { }

    /// <summary>
    /// Represents the Apstars configurator.
    /// </summary>
    public class ApstarsConfigurator : IApstarsConfigurator
    {
        #region Private Fields
        private readonly RegularConfigSource configSource = new RegularConfigSource();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ApstarsConfigurator</c> class.
        /// </summary>
        public ApstarsConfigurator() { }
        #endregion

        #region IConfigurator<IConfigSource> Members
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <returns>The configured container.</returns>
        public RegularConfigSource Configure()
        {
            return configSource;
        }

        #endregion
    }
}
