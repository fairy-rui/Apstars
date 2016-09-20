using System;

namespace Apstars.Interception
{
    /// <summary>
    /// Represents that the decorated classes are requiring a base type
    /// when its interface is proxied by the Castle dynamic proxy mechanism.
    /// </summary>
    /// <remarks>By using this attribute on the class, when the Castle Dynamic
    /// Proxy framework is creating the proxy class for this class' interface,
    /// the base type specified in this attribute will be used as the base type
    /// for the created proxy class.</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BaseTypeForInterfaceProxyAttribute : System.Attribute
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the base type from which the generated proxy object
        /// should derive.
        /// </summary>
        public Type BaseType { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>BaseTypeForInterfaceProxyAttribute</c> class.
        /// </summary>
        /// <param name="baseType">The base type from which the generated proxy object
        /// should derive.</param>
        public BaseTypeForInterfaceProxyAttribute(Type baseType)
        {
            this.BaseType = baseType;
        }
        #endregion
    }
}
