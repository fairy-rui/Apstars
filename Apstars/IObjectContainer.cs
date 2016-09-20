
namespace Apstars
{
    /// <summary>
    /// Represents that the implemented classes are object containers.
    /// </summary>
    public interface IObjectContainer : IServiceLocator
    {
        /// <summary>
        /// Initializes the object container by using the application/web config file.
        /// </summary>
        /// <param name="configSectionName">The name of the ConfigurationSection in the application/web config file
        /// which is used for initializing the object container.</param>
        void InitializeFromConfigFile(string configSectionName);
        /// <summary>
        /// Gets the wrapped container instance.
        /// </summary>
        /// <typeparam name="T">The type of the wrapped container.</typeparam>
        /// <returns>The instance of the wrapped container.</returns>
        T GetWrappedContainer<T>();
    }
}
