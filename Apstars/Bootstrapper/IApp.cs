using Apstars.Config;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;

namespace Apstars.Bootstrapper
{
    /// <summary>
    /// Represents that the implemented classes are Apstars applications. 
    /// </summary>
    public interface IApp
    {
        /// <summary>
        /// Gets the <see cref="Apstars.Config.IConfigSource" /> instance that was used for
        /// configuring the application.
        /// </summary>
        IConfigSource ConfigSource { get; }

        /// <summary>
        /// Gets the <see cref="Apstars.IObjectContainer" /> instance with which the application
        /// registers or resolves the object dependencies.
        /// </summary>
        ObjectContainer ObjectContainer { get; }

        /// <summary>
        /// Gets a list of <see cref="Castle.DynamicProxy.IInterceptor" /> instances that are
        /// registered on the current application.
        /// </summary>
        IEnumerable<IInterceptor> Interceptors { get; }

        /// <summary>
        /// Starts the application. 
        /// </summary>
        void Start();

        /// <summary>
        /// The event that occurs when the application is initializing. 
        /// </summary>
        event EventHandler<AppInitEventArgs> Initialize;
    }
}
