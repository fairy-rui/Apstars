using System;
using System.Collections.Generic;

namespace Apstars.Application.Dto
{
    /// <summary>
    /// Implements <see cref="IListResult{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">Type of the items in the <see cref="Items"/> list</typeparam>
    [Serializable]
    public class ListResultDto<TKey, TEntity> : IListResult<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntityDto<TKey>, new()
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public IReadOnlyList<TEntity> Items
        {
            get { return _items ?? (_items = new List<TEntity>()); }
            set { _items = value; }
        }
        private IReadOnlyList<TEntity> _items;

        /// <summary>
        /// Creates a new <see cref="ListResultDto{TEntity}"/> object.
        /// </summary>
        public ListResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ListResultDto{TEntity}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResultDto(IReadOnlyList<TEntity> items)
        {
            Items = items;
        }
    }

    /// <summary>
    /// Implements <see cref="IListResult{TEntity}"/>.
    /// </summary>    
    /// <typeparam name="TEntity">Type of the items in the <see cref="Items"/> list</typeparam>
    [Serializable]
    public class ListResultDto<TEntity> : ListResultDto<Guid, TEntity>        
        where TEntity : class, IEntityDto<Guid>, new()
    {        
        /// <summary>
        /// Creates a new <see cref="ListResultDto{TEntity}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResultDto(IReadOnlyList<TEntity> items)
            : base(items)
        {
            
        }
    }
}
