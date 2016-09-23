using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Apstars.Bus.DirectBus
{
    /// <summary>
    /// Represents the message bus that will dispatch the messages immediately once
    /// the bus is committed.
    /// </summary>
    public abstract class DirectBus : DisposableObject, IBus
    {
        #region Private Fields
        private volatile bool committed = true;
        private readonly IMessageDispatcher dispatcher;
        private ConcurrentQueue<object> messageQueue = new ConcurrentQueue<object>();
        private readonly object queueLock = new object();
        private object[] backupMessageArray;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>DirectBus&lt;TMessage&gt;</c> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="Apstars.Bus.IMessageDispatcher"/> which
        /// dispatches messages in the bus.</param>
        protected DirectBus(IMessageDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
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
        }
        #endregion

        #region IBus Members
        /// <summary>
        /// Publishes the specified message to the bus.
        /// </summary>
        /// <param name="message">The message to be published.</param>
        public void Publish<TMessage>(TMessage message)
        {
            messageQueue.Enqueue(message);
            committed = false;
        }
        /// <summary>
        /// Publishes a collection of messages to the bus.
        /// </summary>
        /// <param name="messages">The messages to be published.</param>
        public void Publish<TMessage>(IEnumerable<TMessage> messages)
        {
            foreach (var message in messages)
            {
                messageQueue.Enqueue(message);
            }
            committed = false;
        }
        /// <summary>
        /// Clears the published messages waiting for commit.
        /// </summary>
        public void Clear()
        {
            Interlocked.Exchange<ConcurrentQueue<object>>(ref messageQueue, new ConcurrentQueue<object>());
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public bool DistributedTransactionSupported
        {
            get { return false; }
        }
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work was successfully committed.
        /// </summary>
        public bool Committed
        {
            get { return this.committed; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Commit()
        {
            backupMessageArray = new object[messageQueue.Count];
            messageQueue.CopyTo(backupMessageArray, 0);
            while (messageQueue.Count > 0)
            {
                object result;
                if (messageQueue.TryDequeue(out result))
                {
                    dispatcher.DispatchMessage(result);
                }
            }
            committed = true;
        }

        public Task CommitAsync()
        {
            return CommitAsync(CancellationToken.None);
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(Commit, cancellationToken);
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Rollback()
        {
            if (backupMessageArray != null && backupMessageArray.Length > 0)
            {
                Clear();
                foreach (var msg in backupMessageArray)
                {
                    messageQueue.Enqueue(msg);
                }
            }
            committed = false;
        }
        #endregion
    }
}
