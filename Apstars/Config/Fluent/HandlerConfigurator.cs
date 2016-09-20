using System;

namespace Apstars.Config.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are message handler configurators.
    /// </summary>
    public interface IHandlerConfigurator : IConfigSourceConfigurator { }

    /// <summary>
    /// Represents the message handler configurator.
    /// </summary>
    public class HandlerConfigurator : ConfigSourceConfigurator, IHandlerConfigurator
    {
        #region Private Fields
        private readonly string name;
        private readonly HandlerKind handlerKind;
        private readonly HandlerSourceType sourceType;
        private readonly string source;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>HandlerConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="name">The name of the message handler.</param>
        /// <param name="handlerKind">The <see cref="HandlerKind"/> which specifies the kind of the handler, can either be a Command or an Event.</param>
        /// <param name="sourceType">The <see cref="HandlerSourceType"/> which specifies the type of the source, can either be an Assembly or a Type.</param>
        /// <param name="source">The source name, if <paramref name="sourceType"/> is Assembly, the source name should be the assembly full name, if
        /// <paramref name="sourceType"/> is Type, the source name should be the assembly qualified name of the type.</param>
        public HandlerConfigurator(IConfigSourceConfigurator context, string name, HandlerKind handlerKind,
            HandlerSourceType sourceType, string source)
            : base(context)
        {
            this.name = name;
            this.handlerKind = handlerKind;
            this.sourceType = sourceType;
            this.source = source;
        }
        /// <summary>
        /// Initializes a new instance of <c>HandlerConfigurator</c> class.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        /// <param name="handlerKind">The <see cref="HandlerKind"/> which specifies the kind of the handler, can either be a Command or an Event.</param>
        /// <param name="sourceType">The <see cref="HandlerSourceType"/> which specifies the type of the source, can either be an Assembly or a Type.</param>
        /// <param name="source">The source name, if <paramref name="sourceType"/> is Assembly, the source name should be the assembly full name, if
        /// <paramref name="sourceType"/> is Type, the source name should be the assembly qualified name of the type.</param>
        public HandlerConfigurator(IConfigSourceConfigurator context, HandlerKind handlerKind,
            HandlerSourceType sourceType, string source)
            : this(context, Guid.NewGuid().ToString(), handlerKind, sourceType, source) { }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The configuration container.</param>
        /// <returns>The configured container.</returns>
        protected override RegularConfigSource DoConfigure(RegularConfigSource container)
        {
            container.AddHandler(this.name, this.handlerKind, this.sourceType, this.source);
            return container;
        }
        #endregion
    }
}
