using System.Threading;
using System.Threading.Tasks;

namespace Apstars
{
    /// <summary>
    /// 表示所有集成于该接口的类型都是Unit Of Work的一种实现。
    /// </summary>
    /// <remarks>有关Unit Of Work的详细信息，请参见UnitOfWork模式：http://martinfowler.com/eaaCatalog/unitOfWork.html。
    /// </remarks>
    public interface IUnitOfWork //: IDependency
    {
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值，该值表示当前的Unit Of Work是否支持Microsoft分布式事务处理机制。
        /// </summary>
        bool DistributedTransactionSupported { get; }
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值，该值表述了当前的Unit Of Work事务是否已被提交。
        /// </summary>
        bool Committed { get; }
        /// <summary>
        /// 提交当前的Unit Of Work事务。
        /// </summary>
        void Commit();

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <returns>The task that performs the commit operation.</returns>
        Task CommitAsync();

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> object which propagates notification that operations should be canceled.</param>
        /// <returns>The task that performs the commit operation.</returns>
        Task CommitAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 回滚当前的Unit Of Work事务。
        /// </summary>
        void Rollback();
    }
}
