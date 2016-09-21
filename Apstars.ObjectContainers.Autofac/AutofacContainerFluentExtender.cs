using Apstars.Config.Fluent;
using Autofac;
using Autofac.Core;
using System;
using System.Reflection;

namespace Apstars.ObjectContainers.Autofac
{
    /// <summary>
    /// Represents the Extension Method provider which provides additional APIs
    /// for using Autofac container, based on the existing Apstars Fluent API routines.
    /// </summary>
    public static class AutofacContainerFluentExtender
    {
        #region IApstarsConfigurator Extensions
        /// <summary>
        /// Configures the Apstars framework by using Autofac as the object container and other settings by default.
        /// </summary>
        /// <param name="configurator">The <see cref="IApstarsConfigurator"/> instance to be extended.</param>
        /// <param name="containerInitFromConfigFile">The <see cref="System.Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="containerConfigSectionName">The name of the section in the config file. This value must be specified when the <paramref name="containerInitFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instace.</returns>
        public static IObjectContainerConfigurator UsingAutofacContainerWithDefaultSettings(this IApstarsConfigurator configurator, bool containerInitFromConfigFile = false, string containerConfigSectionName = null)
        {
            return configurator.WithDefaultSettings().UsingAutofacContainer(containerInitFromConfigFile, containerConfigSectionName);
        }
        #endregion

        #region ISequenceGeneratorConfigurator Extensions
        /// <summary>
        /// Configures the Apstars framework by using Autofac as the object container.
        /// </summary>
        /// <param name="configurator">The <see cref="ISequenceGeneratorConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="System.Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instace.</returns>
        public static IObjectContainerConfigurator UsingAutofacContainer(this ISequenceGeneratorConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
        {
            return configurator.UsingObjectContainer<AutofacObjectContainer>(initFromConfigFile, sectionName);
        }
        #endregion

        #region IHandlerConfigurator Extensions
        /// <summary>
        /// Configures the Apstars framework by using Autofac as the object container.
        /// </summary>
        /// <param name="configurator">The <see cref="IHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="System.Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instace.</returns>
        public static IObjectContainerConfigurator UsingAutofacContainer(this IHandlerConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
        {
            return configurator.UsingObjectContainer<AutofacObjectContainer>(initFromConfigFile, sectionName);
        }
        #endregion

        #region IExceptionHandlerConfigurator Extensions
        /// <summary>
        /// Configures the Apstars framework by using Autofac as the object container.
        /// </summary>
        /// <param name="configurator">The <see cref="IExceptionHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="System.Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instace.</returns>
        public static IObjectContainerConfigurator UsingAutofacContainer(this IExceptionHandlerConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
        {
            return configurator.UsingObjectContainer<AutofacObjectContainer>(initFromConfigFile, sectionName);
        }
        #endregion

        #region IInterceptionConfigurator Extensions
        /// <summary>
        /// Configures the Apstars framework by using Autofac as the object container.
        /// </summary>
        /// <param name="configurator">The <see cref="IInterceptionConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="System.Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instace.</returns>
        public static IObjectContainerConfigurator UsingAutofacContainer(this IInterceptionConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
        {
            return configurator.UsingObjectContainer<AutofacObjectContainer>(initFromConfigFile, sectionName);
        }
        #endregion

        #region AutofacObjectContainer Extensions
        /// <summary>
        /// Configures the Autofac by using Module scanning.
        /// </summary>
        /// <param name="container">The <see cref="AutofacObjectContainer"/> instance to be extended.</param>
        /// <param name="assemblies">The assemblies from which to register modules.</param>
        /// <returns>The <see cref="AutofacObjectContainer"/> instace.</returns>
        public static ObjectContainer RegisterAssemblyModules(this ObjectContainer container, params Assembly[] assemblies)
        {
            if (container == null) throw new ArgumentNullException(container.GetType().Name);

            (container as AutofacObjectContainer).Builder.RegisterAssemblyModules(assemblies);
            return container;
        }
        /// <summary>
        /// Configures the Autofac by using Module scanning.
        /// </summary>
        /// <param name="container">The <see cref="AutofacObjectContainer"/> instance to be extended.</param>
        /// <param name="moduleType">The <see cref="Type"/> of the module to add.</param>
        /// <param name="assemblies">The assemblies from which to register modules.</param>
        /// <returns>The <see cref="AutofacObjectContainer"/> instace.</returns>
        public static ObjectContainer RegisterAssemblyModules(this ObjectContainer container, Type moduleType, params Assembly[] assemblies)
        {
            if (container == null) throw new ArgumentNullException(container.GetType().Name);
            if (moduleType == null) throw new ArgumentNullException(moduleType.GetType().Name);

            (container as AutofacObjectContainer).Builder.RegisterAssemblyModules(moduleType, assemblies);
            return container;
        }
        /// <summary>
        /// Configures the Autofac by using Module scanning.
        /// </summary>
        /// <typeparam name="TModule">The type of the module to add.</typeparam>
        /// <param name="container">The <see cref="AutofacObjectContainer"/> instance to be extended.</param>
        /// <param name="assemblies">The assemblies from which to register modules.</param>
        /// <returns>The <see cref="AutofacObjectContainer"/> instace.</returns>
        public static ObjectContainer RegisterAssemblyModules<TModule>(this ObjectContainer container, params Assembly[] assemblies)
            where TModule : IModule
        {
            if (container == null) throw new ArgumentNullException(container.GetType().Name);

            (container as AutofacObjectContainer).Builder.RegisterAssemblyModules<TModule>(assemblies);
            return container;
        }
        /// <summary>
        /// Configures the Autofac.
        /// </summary>
        /// <param name="container">The <see cref="AutofacObjectContainer"/> instance to be extended.</param>
        public static void Build(this ObjectContainer container)
        {
            if (container == null) throw new ArgumentNullException(container.GetType().Name);

            (container as AutofacObjectContainer).Build();
        }
        #endregion
    }
}
