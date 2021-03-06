﻿using Apstars.Bootstrapper;
using Apstars.Config;
using Apstars.Properties;
using System;
using System.Configuration;

namespace Apstars.Generators
{
    /// <summary>
    /// Represents the default sequence generator.
    /// </summary>
    public sealed class SequenceGenerator : ISequenceGenerator
    {
        #region Private Fields
        private static readonly SequenceGenerator instance = new SequenceGenerator();
        private readonly ISequenceGenerator generator = null;
        #endregion

        #region Ctor
        static SequenceGenerator() { }

        private SequenceGenerator()
        {
            try
            {
                if (AppRuntime.Instance.CurrentApplication == null)
                    throw new ApstarsException("The application has not been initialized and started yet.");

                if (AppRuntime.Instance.CurrentApplication.ConfigSource == null ||
                    AppRuntime.Instance.CurrentApplication.ConfigSource.Config == null ||
                    AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators == null ||
                    AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.SequenceGenerator == null ||
                    string.IsNullOrEmpty(AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.SequenceGenerator.Provider) ||
                    string.IsNullOrWhiteSpace(AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.SequenceGenerator.Provider))
                {
                    generator = new SequentialIdentityGenerator();
                }
                else
                {
                    Type type = Type.GetType(AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.SequenceGenerator.Provider);
                    if (type == null)
                        throw new ConfigException(string.Format("Unable to create the type from the name {0}.", AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Generators.SequenceGenerator.Provider));
                    if (type.Equals(this.GetType()))
                        throw new ApstarsException("Type {0} cannot be used as sequence generator, it is maintained by the Apstars framework internally.", this.GetType().AssemblyQualifiedName);

                    generator = (ISequenceGenerator)Activator.CreateInstance(type);
                }
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApstarsException(Resources.EX_GET_IDENTITY_GENERATOR_FAIL, ex);
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the singleton instance of <c>SequenceGenerator</c> class.
        /// </summary>
        public static SequenceGenerator Instance
        {
            get { return instance; }
        }
        #endregion

        #region ISequenceGenerator Members
        /// <summary>
        /// Gets the next value of the sequence.
        /// </summary>
        public object Next
        {
            get { return generator.Next; }
        }

        #endregion
    }
}
