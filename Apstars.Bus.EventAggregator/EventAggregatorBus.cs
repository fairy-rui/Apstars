﻿using Apstars.Events;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Apstars.Bus.EventAggregator
{
    /// <summary>
    /// Represents the bus implemented by using the event aggregator.
    /// </summary>
    public abstract class EventAggregatorBus : DisposableObject, IBus
    {
        #region Private Fields
        private ConcurrentQueue<object> messageQueue = new ConcurrentQueue<object>();
        private readonly IEventAggregator eventAggregator;
        private readonly MethodInfo publishMethod;
        private readonly object sync = new object();
        private bool committed = true;
        private object[] backupMessageArray;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>EventAggregatorBus</c> class.
        /// </summary>
        /// <param name="eventAggregator">The <see cref="IEventAggregator"/> instance.</param>
        public EventAggregatorBus(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            publishMethod = (from m in this.eventAggregator.GetType().GetMethods()
                             let parameters = m.GetParameters()
                             let methodName = m.Name
                             where methodName == "Publish" &&
                             parameters != null &&
                             parameters.Length == 1
                             select m).First();
        }
        #endregion

        #region Private Methods
        private void PublishInternal<TMessage>(TMessage message)
        {
            messageQueue.Enqueue(message);
            committed = false;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing) { }
        #endregion

        #region IBus Members
        /// <summary>
        /// Publishes the specified message to the bus.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be published.</typeparam>
        /// <param name="message">The message to be published.</param>
        public void Publish<TMessage>(TMessage message)
        {
            PublishInternal<TMessage>(message);
        }
        /// <summary>
        /// Publishes a collection of messages to the bus.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be published.</typeparam>
        /// <param name="messages">The messages to be published.</param>
        public void Publish<TMessage>(IEnumerable<TMessage> messages)
        {
            foreach (var message in messages)
                PublishInternal<TMessage>(message);
        }
        /// <summary>
        /// Clears the published messages waiting for commit.
        /// </summary>
        public void Clear()
        {
            Interlocked.Exchange<ConcurrentQueue<object>>(ref messageQueue, new ConcurrentQueue<object>());
            committed = true;
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
            get { return committed; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Commit()
        {
            lock (sync)
            {
                backupMessageArray = new object[messageQueue.Count];
                messageQueue.CopyTo(backupMessageArray, 0);
                while (messageQueue.Count > 0)
                {
                    object result;
                    if (messageQueue.TryDequeue(out result))
                    {
                        var @event = result;
                        var @eventType = @event.GetType();
                        var method = publishMethod.MakeGenericMethod(@eventType);
                        method.Invoke(this.eventAggregator, new object[] { @event });
                    }
                }
                committed = true;
            }
        }

        public Task CommitAsync()
        {
            return CommitAsync(CancellationToken.None);
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            // TODO: Might need to change the current implementation
            // to use the ConcurrentQueue instead.
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
