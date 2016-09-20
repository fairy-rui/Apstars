using Apstars.Exceptions;
using Castle.DynamicProxy;
using System;

namespace Apstars.Interception
{
    /// <summary>
    /// Represents the interceptor for handling exceptions.
    /// </summary>
    public class ExceptionHandlingInterceptor : IInterceptor
    {
        #region Private Methods
        private object GetReturnValueByType(Type type)
        {
            if (type.IsClass || type.IsInterface)
                return null;
            if (type == typeof(void))
                return null;
            return Activator.CreateInstance(type);
        }
        #endregion

        #region IInterceptor Members
        /// <summary>
        /// Performs the intercept actions.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                invocation.ReturnValue = GetReturnValueByType(invocation.Method.ReturnType);
                bool handled = ExceptionManager.HandleException(ex);
                if (!handled)
                    throw;
            }
        }

        #endregion
    }
}
