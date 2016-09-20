using Apstars.Bootstrapper;
using Apstars.Interception;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apstars
{
    /// <summary>
    /// Represents the base class for object containers.
    /// </summary>
    public abstract class ObjectContainer : IObjectContainer
    {
        #region Private Fields
        private readonly IInterceptorSelector interceptorSelector = new InterceptorSelector();
        private readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
        private readonly ProxyGenerationOptions proxyGenerationOptions;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ObjectContainer</c> class.
        /// </summary>
        public ObjectContainer()
        {
            this.proxyGenerationOptions = new ProxyGenerationOptions { Selector = interceptorSelector };
        }
        #endregion

        #region Private Methods
        private object GetProxyObject(Type targetType, object targetObject)
        {
            IInterceptor[] interceptors = AppRuntime.Instance.CurrentApplication.Interceptors.ToArray();

            if (interceptors == null ||
                interceptors.Length == 0)
                return targetObject;

            if (targetType.IsInterface)
            {
                object obj = null;
                ProxyGenerationOptions proxyGenerationOptionsForInterface = new ProxyGenerationOptions();
                proxyGenerationOptionsForInterface.Selector = interceptorSelector;
                Type targetObjectType = targetObject.GetType();
                if (targetObjectType.IsDefined(typeof(BaseTypeForInterfaceProxyAttribute), false))
                {
                    BaseTypeForInterfaceProxyAttribute baseTypeForIPAttribute = targetObjectType.GetCustomAttributes(typeof(BaseTypeForInterfaceProxyAttribute), false)[0] as BaseTypeForInterfaceProxyAttribute;
                    proxyGenerationOptionsForInterface.BaseTypeForInterfaceProxy = baseTypeForIPAttribute.BaseType;
                }
                if (targetObjectType.IsDefined(typeof(AdditionalInterfaceToProxyAttribute), false))
                {
                    List<Type> intfTypes = targetObjectType.GetCustomAttributes(typeof(AdditionalInterfaceToProxyAttribute), false)
                                                           .Select(p =>
                                                           {
                                                               AdditionalInterfaceToProxyAttribute attrib = p as AdditionalInterfaceToProxyAttribute;
                                                               return attrib.InterfaceType;
                                                           }).ToList();
                    obj = proxyGenerator.CreateInterfaceProxyWithTarget(targetType, intfTypes.ToArray(), targetObject, proxyGenerationOptionsForInterface, interceptors);
                }
                else
                    obj = proxyGenerator.CreateInterfaceProxyWithTarget(targetType, targetObject, proxyGenerationOptionsForInterface, interceptors);
                return obj;
            }
            else
                return proxyGenerator.CreateClassProxyWithTarget(targetType, targetObject, proxyGenerationOptions, interceptors);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service object.</typeparam>
        /// <returns>The instance of the service object.</returns>
        protected virtual T DoGetService<T>()
            where T : class
        {
            return this.DoGetService(typeof(T)) as T;
        }
        /// <summary>
        /// Gets the service object of the specified type, with overrided
        /// arguments provided.
        /// </summary>
        /// <typeparam name="T">The type of the service object.</typeparam>
        /// <param name="overridedArguments">The overrided arguments to be used when getting the service.</param>
        /// <returns>The instance of the service object.</returns>
        protected virtual T DoGetService<T>(object overridedArguments) where T : class
        {
            return this.DoGetService(typeof(T), overridedArguments) as T;
        }
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object
        /// of type serviceType.</returns>
        protected abstract object DoGetService(Type serviceType);
        /// <summary>
        /// Gets the service object of the specified type, with overrided
        /// arguments provided.
        /// </summary>
        /// <param name="serviceType">The type of the service to get.</param>
        /// <param name="overridedArguments">The overrided arguments to be used when getting the service.</param>
        /// <returns>The instance of the service object.</returns>
        protected abstract object DoGetService(Type serviceType, object overridedArguments);
        /// <summary>
        /// Resolves all the objects from the specified type.
        /// </summary>
        /// <param name="serviceType">The type of the objects to be resolved.</param>
        /// <returns>A <see cref="System.Array"/> object which contains all the objects resolved.</returns>
        protected abstract Array DoResolveAll(Type serviceType);
        /// <summary>
        /// Resolves all the objects from the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be resolved.</typeparam>
        /// <returns>A <see cref="System.Array"/> object which contains all the objects resolved.</returns>
        protected virtual T[] DoResolveAll<T>()
            where T : class
        {
            //return this.DoResolveAll(typeof(T)) as T[];
            var original = this.DoResolveAll(typeof(T));
            var casted = new T[original.Length];
            int index = 0;
            var e = original.GetEnumerator();
            while (e.MoveNext())
            {
                casted[index] = e.Current as T;
                index++;
            }
            return casted;
        }
        #endregion

        #region IObjectContainer Members
        /// <summary>
        /// Initializes the object container by using the application/web config file.
        /// </summary>
        /// <param name="configSectionName">The name of the ConfigurationSection in the application/web config file
        /// which is used for initializing the object container.</param>
        public abstract void InitializeFromConfigFile(string configSectionName);
        /// <summary>
        /// Gets the wrapped container instance.
        /// </summary>
        /// <typeparam name="T">The type of the wrapped container.</typeparam>
        /// <returns>The instance of the wrapped container.</returns>
        public abstract T GetWrappedContainer<T>();
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service object.</typeparam>
        /// <returns>The instance of the service object.</returns>
        public T GetService<T>() where T : class
        {
            T serviceImpl = this.DoGetService<T>();
            return (T)GetProxyObject(typeof(T), serviceImpl);
        }
        /// <summary>
        /// Gets the service object of the specified type, with overrided
        /// arguments provided.
        /// </summary>
        /// <typeparam name="T">The type of the service object.</typeparam>
        /// <param name="overridedArguments">The overrided arguments to be used when getting the service.</param>
        /// <returns>The instance of the service object.</returns>
        public T GetService<T>(object overridedArguments) where T : class
        {
            T serviceImpl = this.DoGetService<T>(overridedArguments);
            return (T)GetProxyObject(typeof(T), serviceImpl);
        }
        /// <summary>
        /// Gets the service object of the specified type, with overrided
        /// arguments provided.
        /// </summary>
        /// <param name="serviceType">The type of the service to get.</param>
        /// <param name="overridedArguments">The overrided arguments to be used when getting the service.</param>
        /// <returns>The instance of the service object.</returns>
        public object GetService(Type serviceType, object overridedArguments)
        {
            object serviceImpl = this.DoGetService(serviceType, overridedArguments);
            return this.GetProxyObject(serviceType, serviceImpl);
        }
        /// <summary>
        /// Resolves all the objects from the specified type.
        /// </summary>
        /// <param name="serviceType">The type of the objects to be resolved.</param>
        /// <returns>A <see cref="System.Array"/> object which contains all the objects resolved.</returns>
        public Array ResolveAll(Type serviceType)
        {
            var serviceImpls = this.DoResolveAll(serviceType);
            List<object> proxiedServiceImpls = new List<object>();
            foreach (var serviceImpl in serviceImpls)
                proxiedServiceImpls.Add(GetProxyObject(serviceType, serviceImpl));
            return proxiedServiceImpls.ToArray();
        }
        /// <summary>
        /// Resolves all the objects from the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be resolved.</typeparam>
        /// <returns>A <see cref="System.Array"/> object which contains all the objects resolved.</returns>
        public T[] ResolveAll<T>() where T : class
        {
            var serviceImpls = this.DoResolveAll<T>();
            List<T> proxiedServiceImpls = new List<T>();
            foreach (var serviceImpl in serviceImpls)
                proxiedServiceImpls.Add((T)GetProxyObject(typeof(T), serviceImpl));
            return proxiedServiceImpls.ToArray();
        }
        #endregion

        #region IServiceProvider Members
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object
        /// of type serviceType.</returns>
        public object GetService(Type serviceType)
        {
            object serviceImpl = this.DoGetService(serviceType);
            return GetProxyObject(serviceType, serviceImpl);
        }

        #endregion

        #region IServiceLocator Members
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the given type
        /// has been registered to the service locator.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>True if the type has been registered, otherwise, false.</returns>
        public abstract bool Registered<T>();
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the given type
        /// has been registered to the service locator.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type has been registered, otherwise, false.</returns>
        public abstract bool Registered(Type type);

        #endregion
    }
}
