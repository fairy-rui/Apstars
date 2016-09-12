using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Apstars.Repositories.EntityFramework
{
    /// <summary>
    /// 由Microsoft Entity Framework支持的一种仓储上下文的实现。
    /// </summary>
    public class EntityFrameworkRepositoryContext : RepositoryContext, IEntityFrameworkRepositoryContext
    {
        #region Private Fields
        private readonly DbContext efContext;
        private readonly object sync = new object();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>EntityFrameworkRepositoryContext</c> class.
        /// </summary>
        /// <param name="efContext">The <see cref="DbContext"/> object that is used when initializing the <c>EntityFrameworkRepositoryContext</c> class.</param>
        public EntityFrameworkRepositoryContext(DbContext efContext)
        {
            this.efContext = efContext;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // The dispose method will no longer be responsible for the commit
                // handling. Since the object container might handle the lifetime
                // of the repository context on the WCF per-request basis, users should
                // handle the commit logic by themselves.
                //if (!committed)
                //{
                //    Commit();
                //}
                efContext.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region IEntityFrameworkRepositoryContext Members
        /// <summary>
        /// Gets the <see cref="DbContext"/> instance handled by Entity Framework.
        /// </summary>
        public DbContext Context
        {
            get { return this.efContext; }
        }
        #endregion

        #region IRepositoryContext Members
        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterNew(object obj)
        {
            this.efContext.Entry(obj).State = System.Data.Entity.EntityState.Added;
            Committed = false;
        }
        /// <summary>
        /// Registers a modified object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterModified(object obj)
        {
            this.efContext.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            Committed = false;
        }
        /// <summary>
        /// Registers a deleted object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public override void RegisterDeleted(object obj)
        {
            this.efContext.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            Committed = false;
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return true; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public override void Commit()
        {
            if (!Committed)
            {
                lock (sync)
                {
                    efContext.SaveChanges();
                }
                Committed = true;
            }
        }

        public override async Task CommitAsync(CancellationToken cancellationToken)
        {
            if (!Committed)
            {
                await efContext.SaveChangesAsync(cancellationToken);
                Committed = true;
            }
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            Committed = false;
        }

        #endregion
    }
}
