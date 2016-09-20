using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Apstars.ObjectContainers.Autofac
{
    /// <summary>
    /// Represents the object container that utilizes the Autofac as
    /// IoC/DI containers.
    /// </summary>
    public class AutofacObjectContainer : ObjectContainer
    {
        #region Private Fields
        private readonly IContainer container;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>AutofacObjectContainer</c> class.
        /// </summary>
        public AutofacObjectContainer()
        {
            //container = new ContainerBuilder().Build();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object
        /// of type serviceType.</returns>
        protected override object DoGetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }
        /// <summary>
        /// Gets the service object of the specified type, with overrided
        /// arguments provided.
        /// </summary>
        /// <param name="serviceType">The type of the service to get.</param>
        /// <param name="overridedArguments">The overrided arguments to be used when getting the service.</param>
        /// <returns>The instance of the service object.</returns>
        protected override object DoGetService(Type serviceType, object overridedArguments)
        {
            List<NamedParameter> overrides = new List<NamedParameter>();
            Type argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property =>
                {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new NamedParameter(propertyName, propertyValue));
                });
            return container.Resolve(serviceType, overrides.ToArray());
        }
        /// <summary>
        /// Resolves all the objects from the specified type.
        /// </summary>
        /// <param name="serviceType">The type of the objects to be resolved.</param>
        /// <returns>A <see cref="System.Array"/> object which contains all the objects resolved.</returns>
        protected override Array DoResolveAll(Type serviceType)
        {
            var typeToResolve = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return container.Resolve(typeToResolve) as Array;
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Initializes the object container from the configuration file, specifying
        /// the name of the configuration section.
        /// </summary>
        /// <param name="configSectionName">The name of the configuration section.</param>
        public override void InitializeFromConfigFile(string configSectionName)
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterModule(new ConfigurationSettingsReader(configSectionName));
           
        }
        /// <summary>
        /// Gets the wrapped container instance.
        /// </summary>
        /// <typeparam name="T">The type of the wrapped container.</typeparam>
        /// <returns>The instance of the wrapped container.</returns>
        public override T GetWrappedContainer<T>()
        {
            if (typeof(T).Equals(typeof(Container)))
                return (T)this.container;
            throw new InfrastructureException("The wrapped container type provided by the current object container should be '{0}'.", typeof(Container));
        }
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the given type
        /// has been registered to the service locator.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>True if the type has been registered, otherwise, false.</returns>
        public override bool Registered<T>()
        {
            return this.container.IsRegistered<T>();
        }
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the given type
        /// has been registered to the service locator.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type has been registered, otherwise, false.</returns>
        public override bool Registered(Type type)
        {
            return this.container.IsRegistered(type);
        }
        
        #endregion
    }
}
