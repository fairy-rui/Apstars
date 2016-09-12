using System;

namespace Apstars
{
    /// <summary>
    /// 表示继承于该接口的类型是领域实体类。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 设置或获取当前领域实体类的全局唯一标识。
        /// </summary>        
        TKey ID { get; set; }
    }

    /// <summary>
    /// 表示继承于该接口的类型是领域实体类。
    /// </summary>
    public interface IEntity : IEntity<int>
    {

    }   
}
