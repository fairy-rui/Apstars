
namespace Apstars
{
    using System;

    /// <summary>
    /// 表示继承于该接口的类型是聚合根类型。
    /// </summary>
    public interface IAggregateRoot<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

    }
    /// <summary>
    /// 表示继承于该接口的类型是聚合根类型。
    /// </summary>
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {

    }
}
