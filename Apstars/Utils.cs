﻿using Apstars.Serialization;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Apstars
{
    /// <summary>
    /// Represents the utility class used by Apstars.
    /// </summary>
    public static class Utils
    {
        #region Private Constants
        private const int InitialPrime = 23;
        private const int FactorPrime = 29;
        #endregion

        #region Extension Methods
        /// <summary>
        /// Gets the signature string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The signature string.</returns>
        public static string GetSignature(this Type type)
        {
            StringBuilder sb = new StringBuilder();

            if (type.IsGenericType)
            {
                sb.Append(type.GetGenericTypeDefinition().FullName);
                sb.Append("[");
                int i = 0;
                var genericArgs = type.GetGenericArguments();
                foreach (var genericArg in genericArgs)
                {
                    sb.Append(genericArg.GetSignature());
                    if (i != genericArgs.Length - 1)
                        sb.Append(", ");
                    i++;
                }
                sb.Append("]");
            }
            else
            {
                if (!string.IsNullOrEmpty(type.FullName))
                    sb.Append(type.FullName);
                else if (!string.IsNullOrEmpty(type.Name))
                    sb.Append(type.Name);
                else
                    sb.Append(type.ToString());

            }
            return sb.ToString();
        }
        /// <summary>
        /// Gets the signature string.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The signature string.</returns>
        public static string GetSignature(this MethodInfo method)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(method.ReturnType.GetSignature());
            sb.Append(" ");
            sb.Append(method.Name);
            if (method.IsGenericMethod)
            {
                sb.Append("[");
                var genericTypes = method.GetGenericArguments();
                int i = 0;
                foreach (var genericType in genericTypes)
                {
                    sb.Append(genericType.GetSignature());
                    if (i != genericTypes.Length - 1)
                        sb.Append(", ");
                    i++;
                }
                sb.Append("]");
            }
            sb.Append("(");
            var parameters = method.GetParameters();
            if (parameters != null && parameters.Length > 0)
            {
                int i = 0;
                foreach (var parameter in parameters)
                {
                    sb.Append(parameter.ParameterType.GetSignature());
                    if (i != parameters.Length - 1)
                        sb.Append(", ");
                    i++;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
        /// <summary>
        /// Deserializes an object from the given byte stream.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="type">The type of the object to be deserialized.</param>
        /// <param name="stream">The byte stream that contains the data of the object.</param>
        /// <returns>The deserialized object.</returns>
        public static object Deserialize(this IObjectSerializer serializer, Type type, byte[] stream)
        {
            var deserializeMethodInfo = serializer.GetType().GetMethod("Deserialize");
            return deserializeMethodInfo.MakeGenericMethod(type).Invoke(serializer, new object[] { stream });
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the hash code for an object based on the given array of hash
        /// codes from each property of the object.
        /// </summary>
        /// <param name="hashCodesForProperties">The array of the hash codes
        /// that are from each property of the object.</param>
        /// <returns>The hash code.</returns>
        public static int GetHashCode(params int[] hashCodesForProperties)
        {
            unchecked
            {
                int hash = InitialPrime;
                foreach (var code in hashCodesForProperties)
                    hash = hash * FactorPrime + code;
                return hash;
            }
        }
        /// <summary>
        /// Generates a unique identifier represented by a <see cref="System.String"/> value
        /// with the specified length.
        /// </summary>
        /// <param name="length">The length of the identifier to be generated.</param>
        /// <returns>The unique identifier represented by a <see cref="System.String"/> value.</returns>
        public static string GetUniqueIdentifier(int length)
        {
            int maxSize = length;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            // Unique identifiers cannot begin with 0-9
            if (result[0] >= '0' && result[0] <= '9')
            {
                return GetUniqueIdentifier(length);
            }
            return result.ToString();
        }

        /// <summary>
        /// Builds the identifier equals predicate that will be used by any LINQ providers to check
        /// if the two aggregate roots are having the same identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="id">The identifier value to be checked with.</param>
        /// <returns>The generated Lambda expression.</returns>
        public static Expression<Func<TAggregateRoot, bool>> BuildIdEqualsPredicate<TKey, TAggregateRoot>(TKey id)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : IAggregateRoot<TKey>
        {
            var parameter = Expression.Parameter(typeof(TAggregateRoot));
            return
                Expression.Lambda<Func<TAggregateRoot, bool>>(
                    Expression.Equal(Expression.Property(parameter, "ID"), Expression.Constant(id)),
                    parameter);
        }

        #endregion
    }
}
