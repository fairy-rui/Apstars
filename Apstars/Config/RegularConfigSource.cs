using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Apstars.Config
{
    /// <summary>
    /// Represents the configuration source that uses the programming
    /// code to initialize the Apstars configuration section.
    /// </summary>
    public sealed class RegularConfigSource : IConfigSource
    {
        #region Private Fields
        private readonly ApstarsConfigSection config;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>RegularConfigSource</c> class.
        /// </summary>
        public RegularConfigSource()
        {
            config = new ApstarsConfigSection();
            config.Application = new ApplicationElement();
            config.Exceptions = new ExceptionElementCollection();
            config.Generators = new GeneratorElement();
            config.Generators.IdentityGenerator = new IdentityGeneratorElement();
            config.Generators.SequenceGenerator = new SequenceGeneratorElement();
            config.Handlers = new HandlerElementCollection();
            config.ObjectContainer = new ObjectContainerElement();
            config.Serializers = new SerializerElement();
            config.Serializers.EventSerializer = new EventSerializerElement();
            config.Serializers.SnapshotSerializer = new SnapshotSerializerElement();
            config.Interception = new InterceptionElement();
            config.Interception.Contracts = new InterceptContractElementCollection();
            config.Interception.Interceptors = new InterceptorElementCollection();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the type of the <see cref="Apstars.Bootstrapper.IApp"/> instance.
        /// </summary>
        public Type Application
        {
            get { return Type.GetType(config.Application.Provider); }
            set { config.Application.Provider = value.AssemblyQualifiedName; }
        }
        /// <summary>
        /// Gets or sets the type of the identity generator.
        /// </summary>
        public Type IdentityGenerator
        {
            get { return Type.GetType(config.Generators.IdentityGenerator.Provider); }
            set { config.Generators.IdentityGenerator.Provider = value.AssemblyQualifiedName; }
        }
        /// <summary>
        /// Gets or sets the type of the sequence generator.
        /// </summary>
        public Type SequenceGenerator
        {
            get { return Type.GetType(config.Generators.SequenceGenerator.Provider); }
            set { config.Generators.SequenceGenerator.Provider = value.AssemblyQualifiedName; }
        }
        /// <summary>
        /// Gets or sets the type of the object container.
        /// </summary>
        public Type ObjectContainer
        {
            get { return Type.GetType(config.ObjectContainer.Provider); }
            set { config.ObjectContainer.Provider = value.AssemblyQualifiedName; }
        }

        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value which indicates whether
        /// the object container should be initialized by the application/web configuration file.
        /// </summary>
        public bool InitObjectContainerFromConfigFile
        {
            get { return config.ObjectContainer.InitFromConfigFile; }
            set { config.ObjectContainer.InitFromConfigFile = value; }
        }
        /// <summary>
        /// Gets or sets the name of the ConfigurationSection in the application/web configuration
        /// file with which the object container should be initialized.
        /// </summary>
        public string ObjectContainerSectionName
        {
            get { return config.ObjectContainer.SectionName; }
            set { config.ObjectContainer.SectionName = value; }
        }
        /// <summary>
        /// Gets or sets the type of the event serializer.
        /// </summary>
        public Type EventSerializer
        {
            get { return Type.GetType(config.Serializers.EventSerializer.Provider); }
            set { config.Serializers.EventSerializer.Provider = value.AssemblyQualifiedName; }
        }
        /// <summary>
        /// Gets or sets the type of the snapshot serializer.
        /// </summary>
        public Type SnapshotSerializer
        {
            get { return Type.GetType(config.Serializers.SnapshotSerializer.Provider); }
            set { config.Serializers.SnapshotSerializer.Provider = value.AssemblyQualifiedName; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a command or event handler to the ConfigSource.
        /// </summary>
        /// <param name="name">Name of the handler to be added.</param>
        /// <param name="kind">The <see cref="Apstars.Config.HandlerKind"/> value which
        /// represents the kind of the message.</param>
        /// <param name="sourceType">The <see cref="Apstars.Config.HandlerSourceType"/> value
        /// which indicates where the handler exists. This could be either a type or an assembly.</param>
        /// <param name="source">The identifier of the source, for <c>sourceType==HandlerSourceType.Type</c>,
        /// this value should be the name of the type; for <c>sourceType==HandlerSourceType.Assembly</c>,
        /// this value should be the name of the assembly.</param>
        public void AddHandler(string name, HandlerKind kind, HandlerSourceType sourceType, string source)
        {
            if (this.config.Handlers == null)
                this.config.Handlers = new HandlerElementCollection();
            foreach (HandlerElement he in this.config.Handlers)
            {
                if ((he.Name == name) || (he.Kind == kind && he.SourceType == sourceType && he.Source == source))
                    return;
            }
            this.config.Handlers.Add(new HandlerElement
            {
                Name = name,
                Kind = kind,
                SourceType = sourceType,
                Source = source
            });
        }
        /// <summary>
        /// Adds an exception definition to the ConfigSource.
        /// </summary>
        /// <param name="exceptionType">The type of the exception to be guarded.</param>
        /// <param name="behavior">The exception handling behavior.</param>
        public void AddException(Type exceptionType, ExceptionHandlingBehavior behavior = ExceptionHandlingBehavior.Direct)
        {
            if (this.config.Exceptions == null)
                this.config.Exceptions = new ExceptionElementCollection();
            foreach (ExceptionElement ee in this.config.Exceptions)
            {
                if (ee.Type == exceptionType.AssemblyQualifiedName && ee.Behavior == behavior)
                    return;
            }
            this.config.Exceptions.Add(new ExceptionElement
            {
                Type = exceptionType.AssemblyQualifiedName,
                Behavior = behavior,
                Handlers = new ExceptionHandlerElementCollection()
            });
        }
        /// <summary>
        /// Adds an exception handler definition to the ConfigSource.
        /// </summary>
        /// <param name="exceptionType">The type of the exception being handled.</param>
        /// <param name="handlerType">The type of the exception handler.</param>
        public void AddExceptionHandler(Type exceptionType, Type handlerType)
        {
            foreach (ExceptionElement exceptionElement in this.config.Exceptions)
            {
                if (exceptionElement.Type.Equals(exceptionType.AssemblyQualifiedName))
                {
                    if (exceptionElement.Handlers == null)
                        exceptionElement.Handlers = new ExceptionHandlerElementCollection();
                    foreach (ExceptionHandlerElement exceptionHandlerElement in exceptionElement.Handlers)
                    {
                        if (exceptionHandlerElement.Type.Equals(handlerType.AssemblyQualifiedName))
                            return;
                    }
                    exceptionElement.Handlers.Add(new ExceptionHandlerElement
                    {
                        Type = handlerType.AssemblyQualifiedName
                    });
                }
            }
        }

        /// <summary>
        /// Adds an interceptor to the current ConfigSource.
        /// </summary>
        /// <param name="name">The name of the interceptor.</param>
        /// <param name="interceptorType">The type of the interceptor.</param>
        public void AddInterceptor(string name, Type interceptorType)
        {
            if (!typeof(IInterceptor).IsAssignableFrom(interceptorType))
                throw new ConfigException("Type '{0}' is not an interceptor.", interceptorType);

            if (this.config.Interception == null)
                this.config.Interception = new InterceptionElement();
            if (this.config.Interception.Interceptors == null)
                this.config.Interception.Interceptors = new InterceptorElementCollection();
            foreach (InterceptorElement interceptor in this.config.Interception.Interceptors)
            {
                if (interceptor.Name.Equals(name) || interceptor.Type.Equals(interceptorType.AssemblyQualifiedName))
                    return;
            }
            InterceptorElement interceptorAdd = new InterceptorElement();
            interceptorAdd.Name = name;
            interceptorAdd.Type = interceptorType.AssemblyQualifiedName;
            this.config.Interception.Interceptors.Add(interceptorAdd);
        }
        /// <summary>
        /// Adds an interception reference to the specified contract and the method.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="method">The method.</param>
        /// <param name="name">The name of the interception reference.</param>
        public void AddInterceptorRef(Type contractType, MethodInfo method, string name)
        {
            if (this.config.Interception != null)
            {
                if (this.config.Interception.Contracts != null)
                {
                    foreach (InterceptContractElement interceptContract in this.config.Interception.Contracts)
                    {
                        if (interceptContract.Type.Equals(contractType.AssemblyQualifiedName))
                        {
                            if (interceptContract.Methods != null)
                            {
                                foreach (InterceptMethodElement interceptMethod in interceptContract.Methods)
                                {
                                    if (interceptMethod.Signature.Equals(method.GetSignature()))
                                    {
                                        if (interceptMethod.InterceptorRefs != null)
                                        {
                                            foreach (InterceptorRefElement interceptorRef in interceptMethod.InterceptorRefs)
                                            {
                                                if (interceptorRef.Name.Equals(name))
                                                    return;
                                            }
                                            interceptMethod.InterceptorRefs.Add(new InterceptorRefElement { Name = name });
                                        }
                                        else
                                        {
                                            interceptMethod.InterceptorRefs = new InterceptorRefElementCollection();
                                            interceptMethod.InterceptorRefs.Add(new InterceptorRefElement { Name = name });
                                        }
                                        return;
                                    }
                                }
                                InterceptMethodElement interceptMethodAdd = new InterceptMethodElement();
                                interceptMethodAdd.Signature = method.GetSignature();
                                interceptMethodAdd.InterceptorRefs = new InterceptorRefElementCollection();
                                interceptMethodAdd.InterceptorRefs.Add(new InterceptorRefElement { Name = name });
                                interceptContract.Methods.Add(interceptMethodAdd);
                            }
                            else
                            {
                                interceptContract.Methods = new InterceptMethodElementCollection();
                                InterceptMethodElement interceptMethodAdd = new InterceptMethodElement();
                                interceptMethodAdd.Signature = method.GetSignature();
                                interceptMethodAdd.InterceptorRefs = new InterceptorRefElementCollection();
                                interceptMethodAdd.InterceptorRefs.Add(new InterceptorRefElement { Name = name });
                                interceptContract.Methods.Add(interceptMethodAdd);
                            }
                            return;
                        }
                    }
                    InterceptContractElement interceptContractAdd = new InterceptContractElement();
                    interceptContractAdd.Type = contractType.AssemblyQualifiedName;
                    interceptContractAdd.Methods = new InterceptMethodElementCollection();
                    InterceptMethodElement interceptMethodAddToContract = new InterceptMethodElement();
                    interceptMethodAddToContract.Signature = method.GetSignature();
                    interceptMethodAddToContract.InterceptorRefs = new InterceptorRefElementCollection();
                    interceptMethodAddToContract.InterceptorRefs.Add(new InterceptorRefElement { Name = name });
                    interceptContractAdd.Methods.Add(interceptMethodAddToContract);
                    this.config.Interception.Contracts.Add(interceptContractAdd);
                }
                else
                {
                    this.config.Interception.Contracts = new InterceptContractElementCollection();
                    InterceptContractElement interceptContractAdd = new InterceptContractElement();
                    interceptContractAdd.Type = contractType.AssemblyQualifiedName;
                    interceptContractAdd.Methods = new InterceptMethodElementCollection();
                    InterceptMethodElement interceptMethodAddToContract = new InterceptMethodElement();
                    interceptMethodAddToContract.Signature = method.GetSignature();
                    interceptMethodAddToContract.InterceptorRefs = new InterceptorRefElementCollection();
                    interceptMethodAddToContract.InterceptorRefs.Add(new InterceptorRefElement { Name = name });
                    interceptContractAdd.Methods.Add(interceptMethodAddToContract);
                    this.config.Interception.Contracts.Add(interceptContractAdd);
                }
            }
            else
            {
                this.config.Interception = new InterceptionElement();
                this.config.Interception.Contracts = new InterceptContractElementCollection();
                InterceptContractElement interceptContractAdd = new InterceptContractElement();
                interceptContractAdd.Type = contractType.AssemblyQualifiedName;
                interceptContractAdd.Methods = new InterceptMethodElementCollection();
                InterceptMethodElement interceptMethodAddToContract = new InterceptMethodElement();
                interceptMethodAddToContract.Signature = method.GetSignature();
                interceptMethodAddToContract.InterceptorRefs = new InterceptorRefElementCollection();
                interceptMethodAddToContract.InterceptorRefs.Add(new InterceptorRefElement { Name = name });
                interceptContractAdd.Methods.Add(interceptMethodAddToContract);
                this.config.Interception.Contracts.Add(interceptContractAdd);
            }
        }
        #endregion

        #region IConfigSource Members
        /// <summary>
        /// Gets the instance of <see cref="Apstars.Config.ApstarsConfigSection"/> class.
        /// </summary>
        public ApstarsConfigSection Config
        {
            get { return this.config; }
        }

        #endregion
    }
}
