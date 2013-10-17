using System.Collections;
using System.Collections.Generic;

namespace ImapX.Collections
{
    public abstract class ImapObjectCollection<T> : IEnumerable<T>
    {

        protected ImapClient Client;
        protected List<T> List;

        internal ImapObjectCollection(){}

        protected ImapObjectCollection(ImapClient client)
        {
            Client = client;
            List = new List<T>();
        }

        protected ImapObjectCollection(ImapClient client, IEnumerable<T> items)
        {
            Client = client;
            List = new List<T>(items);
        }

        public T this[int index]
        {
            get
            {
                var result = List[index];
                return result;
            }
        }

        #region Internal list management methods

        internal void AddInternal(T item)
        {
            List.Add(item);
        }

        internal void AddRangeInternal(IEnumerable<T> collection)
        {
            List.AddRange(collection);
        }

        internal void ClearInternal()
        {
            List.Clear();
        }

        internal void RemoveInternal(T item)
        {
            List.Remove(item);
        }

        internal void RemoveAtInternal(int index)
        {
            List.RemoveAt(index);
        }

        #endregion

        #region IEnumerable methods

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        #endregion

    }
}