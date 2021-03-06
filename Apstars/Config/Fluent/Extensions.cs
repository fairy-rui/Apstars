﻿using Apstars.Bootstrapper;
using Apstars.Exceptions;
using Apstars.Generators;
using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents the Extension Method provider which provides the routines for 
    /// the Fluent API of the Apstars framework configuration.
    /// </summary>
    public static class Extensions
    {
        #region IApstarsConfigurator Extenders
        /// <summary>
        /// Configures the Apstars framework by using the specified application object.
        /// </summary>
        /// <typeparam name="TApplication">The type of the application object.</typeparam>
        /// <param name="configurator">The <see cref="IApstarsConfigurator"/> instance to be extended.</param>
        /// <returns>The <see cref="IApplicationConfigurator"/> instance.</returns>
        public static IApplicationConfigurator WithApplication<TApplication>(this IApstarsConfigurator configurator)
            where TApplication : IApp
        {
            return new ApplicationConfigurator(configurator, typeof(TApplication));
        }
        /// <summary>
        /// Configures the Apstars framework by using the default application instance.
        /// </summary>
        /// <param name="configurator">The <see cref="IApstarsConfigurator"/> instance to be extended.</param>
        /// <returns>The <see cref="IApplicationConfigurator"/> instance.</returns>
        public static IApplicationConfigurator WithDefaultApplication(this IApstarsConfigurator configurator)
        {
            return WithApplication<App>(configurator);
        }
        /// <summary>
        /// Configures the Apstars framework by using the default settings: the default application instance, the default
        /// identity generator and the default sequence generator.
        /// </summary>
        /// <param name="configurator">The <see cref="IApstarsConfigurator"/> instance to be extended.</param>
        /// <returns>The <see cref="ISequenceGeneratorConfigurator"/> instance.</returns>
        public static ISequenceGeneratorConfigurator WithDefaultSettings(this IApstarsConfigurator configurator)
        {
            return WithDefaultSequenceGenerator(
                WithDefaultIdentityGenerator(
                WithDefaultApplication(configurator)));
        }
        #endregion

        #region IAppConfigurator Extenders
        /// <summary>
        /// Configures the Apstars framework by using the specified identity generator.
        /// </summary>
        /// <typeparam name="TIdentityGenerator">The type of the identity generator to be used by the framework.</typeparam>
        /// <param name="configurator">The <see cref="IApplicationConfigurator"/> instance to be extended.</param>
        /// <returns>The <see cref="IIdentityGeneratorConfigurator"/> instance.</returns>
        public static IIdentityGeneratorConfigurator WithIdentityGenerator<TIdentityGenerator>(this IApplicationConfigurator configurator)
            where TIdentityGenerator : IIdentityGenerator
        {
            return new IdentityGeneratorConfigurator(configurator, typeof(TIdentityGenerator));
        }
        /// <summary>
        /// Configures the Apstars framework by using a default identity generator.
        /// </summary>
        /// <param name="configurator">The <see cref="IApplicationConfigurator"/> instance to be extended.</param>
        /// <returns>The <see cref="IIdentityGeneratorConfigurator"/> instance.</returns>
        public static IIdentityGeneratorConfigurator WithDefaultIdentityGenerator(this IApplicationConfigurator configurator)
        {
            return WithIdentityGenerator<SequentialIdentityGenerator>(configurator);
        }
        #endregion

        #region IIdentityGeneratorConfigurator Extenders
        /// <summary>
        /// Configures the Apstars framework by using the specified sequence generator.
        /// </summary>
        /// <typeparam name="TSequenceGenerator">The type of the sequence generator to be used by the framework.</typeparam>
        /// <param name="configurator">The <see cref="IIdentityGeneratorConfigurator"/> instance to be extended.</param>
        /// <returns>The <see cref="ISequenceGeneratorConfigurator"/> instance.</returns>
        public static ISequenceGeneratorConfigurator WithSequenceGenerator<TSequenceGenerator>(this IIdentityGeneratorConfigurator configurator)
            where TSequenceGenerator : ISequenceGenerator
        {
            return new SequenceGeneratorConfigurator(configurator, typeof(TSequenceGenerator));
        }
        /// <summary>
        /// Configures the Apstars framework by using a default sequence generator.
        /// </summary>
        /// <param name="configurator">The <see cref="IIdentityGeneratorConfigurator"/> instance to be extended.</param>
        /// <returns>The <see cref="ISequenceGeneratorConfigurator"/> instance.</returns>
        public static ISequenceGeneratorConfigurator WithDefaultSequenceGenerator(this IIdentityGeneratorConfigurator configurator)
        {
            return WithSequenceGenerator<SequentialIdentityGenerator>(configurator);
        }
        #endregion

        #region ISequenceGeneratorConfigurator Extenders
        /// <summary>
        /// Adds a message handler to the Apstars framework. (This operation only applies on CQRS architecture).
        /// </summary>
        /// <param name="configurator">The <see cref="ISequenceGeneratorConfigurator"/> instance to be extended.</param>
        /// <param name="handlerKind">The <see cref="HandlerKind"/> which specifies the kind of the handler, can either be a Command or an Event.</param>
        /// <param name="sourceType">The <see cref="HandlerSourceType"/> which specifies the type of the source, can either be an Assembly or a Type.</param>
        /// <param name="source">The source name, if <paramref name="sourceType"/> is Assembly, the source name should be the assembly full name, if
        /// <paramref name="sourceType"/> is Type, the source name should be the assembly qualified name of the type.</param>
        /// <param name="name">The name of the message handler.</param>
        /// <returns>The <see cref="IHandlerConfigurator"/> instance.</returns>
        public static IHandlerConfigurator AddMessageHandler(this ISequenceGeneratorConfigurator configurator, HandlerKind handlerKind, HandlerSourceType sourceType, string source, string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return new HandlerConfigurator(configurator, handlerKind, sourceType, source);
            else
                return new HandlerConfigurator(configurator, name, handlerKind, sourceType, source);
        }
        /// <summary>
        /// Adds an exception handler to the Apstars framework.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to be handled.</typeparam>
        /// <typeparam name="TExceptionHandler">The type of the exception handler.</typeparam>
        /// <param name="configurator">The <see cref="ISequenceGeneratorConfigurator"/> to be extended.</param>
        /// <param name="behavior">The exception handling behavior.</param>
        /// <returns>The <see cref="IExceptionHandlerConfigurator"/> instance.</returns>
        public static IExceptionHandlerConfigurator AddExceptionHandler<TException, TExceptionHandler>(this ISequenceGeneratorConfigurator configurator, ExceptionHandlingBehavior behavior = ExceptionHandlingBehavior.Direct)
            where TException : Exception
            where TExceptionHandler : IExceptionHandler
        {
            return new ExceptionHandlerConfigurator(configurator, typeof(TException), typeof(TExceptionHandler), behavior);
        }
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="ISequenceGeneratorConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this ISequenceGeneratorConfigurator configurator, MethodInfo interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="ISequenceGeneratorConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this ISequenceGeneratorConfigurator configurator, string interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }
        /// <summary>
        /// Configures the Apstars framework by using the specified object container.
        /// </summary>
        /// <typeparam name="TObjectContainer">The type of the object container to be used by the framework.</typeparam>
        /// <param name="configurator">The <see cref="ISequenceGeneratorConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instance.</returns>
        public static IObjectContainerConfigurator UsingObjectContainer<TObjectContainer>(this ISequenceGeneratorConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
            where TObjectContainer : IObjectContainer
        {
            return new ObjectContainerConfigurator(configurator, typeof(TObjectContainer), initFromConfigFile, sectionName);
        }
        #endregion

        #region IHandlerConfigurator Extenders
        /// <summary>
        /// Adds a message handler to the Apstars framework. (This operation only applies on CQRS architecture).
        /// </summary>
        /// <param name="configurator">The <see cref="IHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="handlerKind">The <see cref="HandlerKind"/> which specifies the kind of the handler, can either be a Command or an Event.</param>
        /// <param name="sourceType">The <see cref="HandlerSourceType"/> which specifies the type of the source, can either be an Assembly or a Type.</param>
        /// <param name="source">The source name, if <paramref name="sourceType"/> is Assembly, the source name should be the assembly full name, if
        /// <paramref name="sourceType"/> is Type, the source name should be the assembly qualified name of the type.</param>
        /// <param name="name">The name of the message handler.</param>
        /// <returns>The <see cref="IHandlerConfigurator"/> instance.</returns>
        public static IHandlerConfigurator AddMessageHandler(this IHandlerConfigurator configurator, HandlerKind handlerKind, HandlerSourceType sourceType, string source, string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return new HandlerConfigurator(configurator, handlerKind, sourceType, source);
            else
                return new HandlerConfigurator(configurator, name, handlerKind, sourceType, source);
        }
        /// <summary>
        /// Adds an exception handler to the Apstars framework.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to be handled.</typeparam>
        /// <typeparam name="TExceptionHandler">The type of the exception handler.</typeparam>
        /// <param name="configurator">The <see cref="IHandlerConfigurator"/> to be extended.</param>
        /// <param name="behavior">The exception handling behavior.</param>
        /// <returns>The <see cref="IExceptionHandlerConfigurator"/> instance.</returns>
        public static IExceptionHandlerConfigurator AddExceptionHandler<TException, TExceptionHandler>(this IHandlerConfigurator configurator, ExceptionHandlingBehavior behavior = ExceptionHandlingBehavior.Direct)
            where TException : Exception
            where TExceptionHandler : IExceptionHandler
        {
            return new ExceptionHandlerConfigurator(configurator, typeof(TException), typeof(TExceptionHandler), behavior);
        }
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="IHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this IHandlerConfigurator configurator, MethodInfo interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="IHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this IHandlerConfigurator configurator, string interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }
        /// <summary>
        /// Configures the Apstars framework by using the specified object container.
        /// </summary>
        /// <typeparam name="TObjectContainer">The type of the object container to be used by the framework.</typeparam>
        /// <param name="configurator">The <see cref="IHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instance.</returns>
        public static IObjectContainerConfigurator UsingObjectContainer<TObjectContainer>(this IHandlerConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
            where TObjectContainer : IObjectContainer
        {
            return new ObjectContainerConfigurator(configurator, typeof(TObjectContainer), initFromConfigFile, sectionName);
        }
        #endregion

        #region IExceptionHandlerConfigurator Extenders
        /// <summary>
        /// Adds an exception handler to the Apstars framework.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to be handled.</typeparam>
        /// <typeparam name="TExceptionHandler">The type of the exception handler.</typeparam>
        /// <param name="configurator">The <see cref="IExceptionHandlerConfigurator"/> to be extended.</param>
        /// <param name="behavior">The exception handling behavior.</param>
        /// <returns>The <see cref="IExceptionHandlerConfigurator"/> instance.</returns>
        public static IExceptionHandlerConfigurator AddExceptionHandler<TException, TExceptionHandler>(this IExceptionHandlerConfigurator configurator, ExceptionHandlingBehavior behavior = ExceptionHandlingBehavior.Direct)
            where TException : Exception
            where TExceptionHandler : IExceptionHandler
        {
            return new ExceptionHandlerConfigurator(configurator, typeof(TException), typeof(TExceptionHandler), behavior);
        }
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="IExceptionHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this IExceptionHandlerConfigurator configurator, MethodInfo interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="IExceptionHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this IExceptionHandlerConfigurator configurator, string interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }
        /// <summary>
        /// Configures the Apstars framework by using the specified object container.
        /// </summary>
        /// <typeparam name="TObjectContainer">The type of the object container to be used by the framework.</typeparam>
        /// <param name="configurator">The <see cref="IExceptionHandlerConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instance.</returns>
        public static IObjectContainerConfigurator UsingObjectContainer<TObjectContainer>(this IExceptionHandlerConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
            where TObjectContainer : IObjectContainer
        {
            return new ObjectContainerConfigurator(configurator, typeof(TObjectContainer), initFromConfigFile, sectionName);
        }
        #endregion

        #region IInterceptionConfigurator Extenders
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="IInterceptionConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this IInterceptionConfigurator configurator, MethodInfo interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }
        /// <summary>
        /// Registers an interceptor on the given method of a given type.
        /// </summary>
        /// <typeparam name="TInterceptor">The type of the interceptor to be registered.</typeparam>
        /// <typeparam name="TContract">The type which contains the method to be intercepted.</typeparam>
        /// <param name="configurator">The <see cref="IInterceptionConfigurator"/> instance to be extended.</param>
        /// <param name="interceptMethod">The method to be intercepted.</param>
        /// <returns>The <see cref="IInterceptionConfigurator"/> instance.</returns>
        public static IInterceptionConfigurator RegisterInterception<TInterceptor, TContract>(this IInterceptionConfigurator configurator, string interceptMethod)
            where TInterceptor : IInterceptor
        {
            return new InterceptionConfigurator(configurator, typeof(TInterceptor), typeof(TContract), interceptMethod);
        }

        /// <summary>
        /// Configures the Apstars framework by using the specified object container.
        /// </summary>
        /// <typeparam name="TObjectContainer">The type of the object container to be used by the framework.</typeparam>
        /// <param name="configurator">The <see cref="IInterceptionConfigurator"/> instance to be extended.</param>
        /// <param name="initFromConfigFile">The <see cref="Boolean"/> value which indicates whether the container configuration should be read from the config file.</param>
        /// <param name="sectionName">The name of the section in the config file. This value must be specified when the <paramref name="initFromConfigFile"/> parameter is set to true.</param>
        /// <returns>The <see cref="IObjectContainerConfigurator"/> instance.</returns>
        public static IObjectContainerConfigurator UsingObjectContainer<TObjectContainer>(this IInterceptionConfigurator configurator, bool initFromConfigFile = false, string sectionName = null)
            where TObjectContainer : IObjectContainer
        {
            return new ObjectContainerConfigurator(configurator, typeof(TObjectContainer), initFromConfigFile, sectionName);
        }
        #endregion

        #region IObjectContainerConfigurator
        /// <summary>
        /// Creates the <see cref="IApp"/> instance.
        /// </summary>
        /// <param name="configurator">The instance of <see cref="IObjectContainerConfigurator"/> to be extended.</param>
        /// <returns>The <see cref="IApp"/> instance.</returns>
        public static IApp Create(this IObjectContainerConfigurator configurator)
        {
            var configSource = configurator.Configure();
            var appInstance = AppRuntime.Create(configSource);
            return appInstance;
        }
        /// <summary>
        /// Creates the <see cref="IApp"/> instance.
        /// </summary>
        /// <param name="configurator">The instance of <see cref="IObjectContainerConfigurator"/> to be extended.</param>
        /// <param name="initializer">The application initializer.</param>
        /// <returns>The <see cref="IApp"/> instance.</returns>
        public static IApp Create(this IObjectContainerConfigurator configurator, EventHandler<AppInitEventArgs> initializer)
        {
            var appInstance = Create(configurator);
            var tmp = initializer;
            if (tmp != null)
                appInstance.Initialize += tmp;
            return appInstance;
        }
        #endregion

        #region AppRuntime Instance Extenders
        /// <summary>
        /// Configures the Apstars framework.
        /// </summary>
        /// <param name="appRuntime">The instance of <see cref="AppRuntime"/> to be extended.</param>
        /// <returns>The <see cref="IApstarsConfigurator"/> instance that holds the configurator for Apstars framework.</returns>
        public static IApstarsConfigurator ConfigureApstars(this AppRuntime appRuntime)
        {
            return new ApstarsConfigurator();
        }
        #endregion
    }
}
