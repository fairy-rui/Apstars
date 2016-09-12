﻿using System;

namespace Apstars.Repositories
{
    /// <summary>
    /// 表示实现该接口的类型是仓储上下文。
    /// </summary>
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        #region Properties
        /// <summary>
        /// 获取仓储上下文的ID。
        /// </summary>
        Guid ID { get; }
        #endregion

        #region Methods
        /// <summary>
        /// 将指定的聚合根标注为“新建”状态。
        /// </summary>
        /// <typeparam name="TAggregateRoot">需要标注状态的聚合根类型。</typeparam>
        /// <param name="obj">需要标注状态的聚合根。</param>
        void RegisterNew(object obj);
        /// <summary>
        /// 将指定的聚合根标注为“更改”状态。
        /// </summary>
        /// <param name="obj">需要标注状态的聚合根。</param>
        void RegisterModified(object obj);
        /// <summary>
        /// 将指定的聚合根标注为“删除”状态。
        /// </summary>
        /// <param name="obj">需要标注状态的聚合根。</param>
        void RegisterDeleted(object obj);
        #endregion
    }
}
