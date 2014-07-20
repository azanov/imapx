using System;
using System.Collections.Generic;
using System.Threading;

namespace ImapX.Collections
{

    public class ThreadSafeQueue<T> where T : class
    {
        readonly object _mutex = new object();
        readonly Queue<T> _queue = new Queue<T>();
        bool _isDead;

        public void Enqueue(T task)
        {
            if (task == null)
                throw new ArgumentNullException("task");
            lock (_mutex)
            {
                if (_isDead)
                    throw new InvalidOperationException("Queue already stopped");
                _queue.Enqueue(task);
                Monitor.Pulse(_mutex);
            }
        }

        public bool TryDequeue(out T item)
        {
            item = null;
            lock (_mutex)
            {
                while (_queue.Count == 0 && !_isDead)
                    Monitor.Wait(_mutex);

                if (_queue.Count == 0)
                    return false;

                item = _queue.Dequeue();
                return true;
            }
        }



        public void Stop()
        {
            lock (_mutex)
            {
                _isDead = true;
                Monitor.PulseAll(_mutex);
            }
        }

    }

}
