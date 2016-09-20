using System;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are exception handler configurators.
    /// </summary>
    public interface IExceptionHandlerConfigurator : IConfigSourceConfigurator { }

    /// <summary>
    /// Represents the exception handler configurator.
    /// </summary>
    public class ExceptionHandlerConfigurator : ConfigSourceConfigurator, IExceptionHandlerConfigurator
    {
        #region Private Fields
        private readonly Type exceptionType;
        private readonly Type exceptionHandlerType;
        private readonly ExceptionHandlingBehavior behavior;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ExceptionHandlerConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="exceptionType">The type of the exception to be handled.</param>
        /// <param name="exceptionHandlerType">The type of the exception handler.</param>
        /// <param name="behavior">The exception handling behavior.</param>
        public ExceptionHandlerConfigurator(IConfigSourceConfigurator context, Type exceptionType, Type exceptionHandlerType, ExceptionHandlingBehavior behavior)
            : base(context)
        {
            this.exceptionType = exceptionType;
            this.exceptionHandlerType = exceptionHandlerType;
            this.behavior = behavior;
        }
        /// <summary>
        /// Initializes a new instance of <c>ExceptionHandlerConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="exceptionType">The type of the exception to be handled.</param>
        /// <param name="exceptionHandlerType">The type of the exception handler.</param>
        public ExceptionHandlerConfigurator(IConfigSourceConfigurator context, Type exceptionType, Type exceptionHandlerType)
            : this(context, exceptionType, exceptionHandlerType, ExceptionHandlingBehavior.Direct)
        { }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The configuration container.</param>
        /// <returns>The configured container.</returns>
        protected override RegularConfigSource DoConfigure(RegularConfigSource container)
        {
            container.AddException(this.exceptionType, behavior);
            container.AddExceptionHandler(this.exceptionType, this.exceptionHandlerType);
            return container;
        }
        #endregion
    }
}
