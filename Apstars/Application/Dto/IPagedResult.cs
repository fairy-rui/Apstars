
using System;

namespace Apstars.Application.Dto
{
    /// <summary>
    /// This interface is defined to standardize to return a page of items to clients.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TKey">Type of the items in the <see cref="IListResult{TEntity}.Items"/> list</typeparam>
    public interface IPagedResult<TKey, TEntity> : IListResult<TKey, TEntity>, IHasTotalCount
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntityDto<TKey>, new()
    {

    }

    /// <summary>
    /// This interface is defined to standardize to return a page of items to clients.
    /// </summary>    
    /// <typeparam name="TKey">Type of the items in the <see cref="IListResult{TEntity}.Items"/> list</typeparam>
    public interface IPagedResult<TEntity> : IListResult<Guid, TEntity>, IHasTotalCount        
        where TEntity : class, IEntityDto<Guid>, new()
    {

    }
}
