using System;
using System.Reflection;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are interception configurators.
    /// </summary>
    public interface IInterceptionConfigurator : IConfigSourceConfigurator { }

    /// <summary>
    /// Represents the interception configurator.
    /// </summary>
    public class InterceptionConfigurator : ConfigSourceConfigurator, IInterceptionConfigurator
    {
        #region Private Fields
        private readonly Type interceptorType;
        private readonly Type contractType;
        private readonly MethodInfo interceptMethod;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>InterceptionConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="interceptorType">The type of the interceptor to be registered.</param>
        /// <param name="contractType">The type that needs to be intercepted.</param>
        /// <param name="interceptMethod">The <see cref="MethodInfo"/> instance that needs to be intercepted.</param>
        public InterceptionConfigurator(IConfigSourceConfigurator context, Type interceptorType, Type contractType, MethodInfo interceptMethod)
            : base(context)
        {
            this.interceptorType = interceptorType;
            this.contractType = contractType;
            this.interceptMethod = interceptMethod;
        }
        /// <summary>
        /// Initializes a new instance of <c>InterceptionConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="interceptorType">The type of the interceptor to be registered.</param>
        /// <param name="contractType">The type that needs to be intercepted.</param>
        /// <param name="interceptMethod">The name of the method that needs to be intercepted.</param>
        public InterceptionConfigurator(IConfigSourceConfigurator context, Type interceptorType, Type contractType, string interceptMethod)
            : base(context)
        {
            this.interceptorType = interceptorType;
            this.contractType = contractType;
            var method = contractType.GetMethod(interceptMethod, BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
                this.interceptMethod = method;
            else
                throw new ConfigException("The method {0} requested doesn't exist in type {1}.", interceptMethod, contractType);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The configuration container.</param>
        /// <returns>The configured container.</returns>
        protected override RegularConfigSource DoConfigure(RegularConfigSource container)
        {
            var name = this.interceptorType.FullName;
            container.AddInterceptor(name, this.interceptorType);
            container.AddInterceptorRef(this.contractType, this.interceptMethod, name);
            return container;
        }
        #endregion
    }
}
