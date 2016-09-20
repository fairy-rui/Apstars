using System.Configuration;
using System.Linq;

namespace Apstars.Config
{
    /// <summary>
    /// Represents the configuration source that uses the application/web configuration file.
    /// </summary>
    public class AppConfigSource : IConfigSource
    {
        #region Private Fields
        private ApstarsConfigSection config;
        #endregion

        #region Public Constants
        /// <summary>
        /// Represents the default name of the configuration section used by Apstars framework.
        /// </summary>
        public const string DefaultConfigSection = "apstars";
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>AppConfigSource</c> class.
        /// </summary>
        public AppConfigSource()
        {
            string configSection = DefaultConfigSection;
            try
            {
                object[] apstarsConfigAttributes = typeof(ApstarsConfigSection).GetCustomAttributes(false);
                if (apstarsConfigAttributes.Any(p => p.GetType().Equals(typeof(System.Xml.Serialization.XmlRootAttribute))))
                {
                    System.Xml.Serialization.XmlRootAttribute xmlRootAttribute = (System.Xml.Serialization.XmlRootAttribute)
                        apstarsConfigAttributes.SingleOrDefault(p => p.GetType().Equals(typeof(System.Xml.Serialization.XmlRootAttribute)));
                    if (!string.IsNullOrEmpty(xmlRootAttribute.ElementName) &&
                        !string.IsNullOrWhiteSpace(xmlRootAttribute.ElementName))
                    {
                        configSection = xmlRootAttribute.ElementName;
                    }
                }
            }
            catch // If any exception occurs, suppress the exception to uuse the default config section.
            {
            }
            LoadConfig(configSection);
        }
        /// <summary>
        /// Initializes a new instance of <c>AppConfigSource</c> class.
        /// </summary>
        /// <param name="configSectionName">The name of the Configuration Section.</param>
        public AppConfigSource(string configSectionName)
        {
            LoadConfig(configSectionName);
        }
        #endregion

        #region Private Methods
        private void LoadConfig(string configSection)
        {
            this.config = (ApstarsConfigSection)ConfigurationManager.GetSection(configSection);
        }
        #endregion

        #region IConfigSource Members
        /// <summary>
        /// Gets the instance of <see cref="Apstars.Config.ApstarsConfigSection"/> class.
        /// </summary>
        public ApstarsConfigSection Config
        {
            get { return this.config; }
        }

        #endregion
    }
}
