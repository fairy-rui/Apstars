using Apstars.Config;
using System;

namespace Apstars.Bootstrapper
{
    /// <summary>
    /// Represents the Application Runtime from where the application is created, initialized and started. 
    /// </summary>
    public sealed class AppRuntime
    {
        #region Private Static Fields

        private static readonly AppRuntime instance = new AppRuntime();
        private static readonly object lockObj = new object();

        #endregion Private Static Fields

        #region Private Fields

        private IApp currentApplication = null;

        #endregion Private Fields

        #region Ctor

        static AppRuntime()
        {
        }

        private AppRuntime()
        {
        }

        #endregion Ctor

        #region Public Static Properties

        /// <summary>
        /// Gets the instance of the current <c> ApplicationRuntime </c> class. 
        /// </summary>
        public static AppRuntime Instance
        {
            get { return instance; }
        }

        #endregion Public Static Properties

        #region Public Properties

        /// <summary>
        /// Gets the instance of the currently running application. 
        /// </summary>
        public IApp CurrentApplication
        {
            get { return currentApplication; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates and initializes a new application instance. 
        /// </summary>
        /// <param name="configSource">
        /// The <see cref="Apstars.Config.IConfigSource" /> instance that is used for initializing
        /// the application.
        /// </param>
        /// <returns> The initialized application instance. </returns>
        public static IApp Create(IConfigSource configSource)
        {
            lock (lockObj)
            {
                if (instance.currentApplication == null)
                {
                    lock (lockObj)
                    {
                        if (configSource.Config == null ||
                            configSource.Config.Application == null)
                            throw new ConfigException("Either apstars configuration or apstars application configuration has not been initialized in the ConfigSource instance.");
                        string typeName = configSource.Config.Application.Provider;
                        if (string.IsNullOrEmpty(typeName))
                            throw new ConfigException("The provider type has not been defined in the ConfigSource.");
                        Type type = Type.GetType(typeName);
                        if (type == null)
                            throw new InfrastructureException("The application provider defined by type '{0}' doesn't exist.", typeName);
                        instance.currentApplication = (IApp)Activator.CreateInstance(type, new object[] { configSource });
                    }
                }
            }
            return instance.currentApplication;
        }

        #endregion Public Methods
    }
}
