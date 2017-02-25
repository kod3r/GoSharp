﻿using System.Collections.Generic;
using System.Threading;

namespace GoSharp.Impl
{
    internal sealed class TransferQueue
    {
        private readonly Queue<TransferQueueItem> _transferQueue;

        internal TransferQueue()
        {
            _transferQueue = new Queue<TransferQueueItem>(100);
        }

        internal void Enqueue(TransferQueueItem transferQueueItem)
        {
            _transferQueue.Enqueue(transferQueueItem);
        }

        internal bool TryDequeue(out TransferQueueItem transferQueueItem)
        {
            transferQueueItem = null;

            while (_transferQueue.Count > 0)
            {
                var tqi = _transferQueue.Dequeue();

                if (tqi.SelectFireContext == null ||
                    Interlocked.CompareExchange(ref tqi.SelectFireContext.Fired, tqi, null) == null)
                {
                    transferQueueItem = tqi;
                    return true;
                }
            }

            return false;
        }

        internal void DequeueNotifyAll()
        {
            TransferQueueItem tqi;
            while (TryDequeue(out tqi))
            {
                tqi.ChannelOperation.Notify();
            }
        }
    }
}
