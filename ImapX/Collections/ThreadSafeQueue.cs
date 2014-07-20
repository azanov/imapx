using System;
using System.Collections.Generic;
using System.Threading;

namespace ImapX.Collections
{

    /// <see cref="http://hashcode.ru/research/221526/%D0%BC%D0%BD%D0%BE%D0%B3%D0%BE%D0%BF%D0%BE%D1%82%D0%BE%D1%87%D0%BD%D0%BE%D1%81%D1%82%D1%8C-%D0%B8%D0%BC%D0%BF%D0%BB%D0%B5%D0%BC%D0%B5%D0%BD%D1%82%D0%B0%D1%86%D0%B8%D1%8F-producer-consumer-pattern"/>
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
