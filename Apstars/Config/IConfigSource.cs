
namespace Apstars.Config
{
    /// <summary>
    /// Represents that the implemented classes are configuration sources for Apstars framework.
    /// </summary>
    public interface IConfigSource
    {
        /// <summary>
        /// Gets the instance of <see cref="Apstars.Config.ApstarsConfigSection"/> class.
        /// </summary>
        ApstarsConfigSection Config { get; }
    }
}
