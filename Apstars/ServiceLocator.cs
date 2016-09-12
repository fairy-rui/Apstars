using Autofac;
using System;
using System.Collections.Generic;

namespace Apstars
{
    /// <summary>
    /// Represents the service locator which locates a service with the given type.
    /// </summary>
    public sealed class ServiceLocator : IServiceLocator
    {
        #region Private Fields
        private IContainer _container;
        #endregion

        #region Private Static Fields
        private static readonly ServiceLocator instance = new ServiceLocator();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ServiceLocator</c> class.
        /// </summary>
        private ServiceLocator()
        {            
            //_container = new AutofacContainer();
            //_container.Init();
        }
        #endregion

        #region Public Static Properties
        /// <summary>
        /// Gets the singleton instance of the <c>ServiceLocator</c> class.
        /// </summary>
        public static ServiceLocator Instance
        {
            get { return instance; }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the singleton instance of the <c>ServiceLocator</c> class.
        /// </summary>
        public IContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the service instance with the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetService<T>()
        {
            return _container.Resolve<T>();
        }
        /// <summary>
        /// Resolves all the objects from the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be resolved.</typeparam>
        /// <returns>A <see cref="System.Array"/> object which contains all the objects resolved.</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.Resolve<IEnumerable<T>>();
        }
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the given type
        /// has been registered to the service locator.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>True if the type has been registered, otherwise, false.</returns>
        public bool Registered<T>()
        {
            return _container.IsRegistered<T>();
        }
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the given type
        /// has been registered to the service locator.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type has been registered, otherwise, false.</returns>
        public bool Registered(Type type)
        {
            return _container.IsRegistered(type);
        }
        #endregion

        #region IServiceProvider Members
        /// <summary>
        /// Gets the service instance with the given type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>The service instance.</returns>
        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        #endregion
    }
}
