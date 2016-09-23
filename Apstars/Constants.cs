
namespace Apstars
{
    /// <summary>
    /// Represents the utility class which provides all the constants used by Apstars framework.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Represents the utility class which provides all the constants for Apstars configuration module.
        /// </summary>
        public static class Configuration
        {
            /// <summary>
            /// The name of the configuration section which holds the configuration elements.
            /// </summary>
            public const string ConfigurationSectionName = @"apstarsConfiguration";
            /// <summary>
            /// The name of the default identity generator.
            /// </summary>
            public const string DefaultIdentityGeneratorName = @"defaultIdentityGenerator";
            /// <summary>
            /// The name of the default sequence generator.
            /// </summary>
            public const string DefaultSequenceGeneratorName = @"defaultSequenceGenerator";
        }
        /// <summary>
        /// Represents the constants and readonly fields used during the running of the application.
        /// </summary>
        public static class ApplicationRuntime
        {
            /// <summary>
            /// Represents the default version number.
            /// </summary>
            public static readonly long DefaultVersion = 0;
            /// <summary>
            /// Represents the default branch number.
            /// </summary>
            public static readonly long DefaultBranch = 0;
        }
    }
}
