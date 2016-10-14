using System;
using System.Collections.Generic;

namespace Apstars.Application.Dto
{
    /// <summary>
    /// This interface is defined to standardize to return a list of items to clients.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">Type of the items in the <see cref="Items"/> list</typeparam>
    public interface IListResult<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntityDto<TKey>, new()
    {
        /// <summary>
        /// List of items.
        /// </summary>
        IReadOnlyList<TEntity> Items { get; set; }
    }

    /// <summary>
    /// This interface is defined to standardize to return a list of items to clients.
    /// </summary>   
    /// <typeparam name="TEntity">Type of the items in the <see cref="Items"/> list</typeparam>
    public interface IListResult<TEntity> : IListResult<Guid, TEntity>
        where TEntity : class, IEntityDto<Guid>, new()
    {

    }
}
