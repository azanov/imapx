using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Collections
{
    public abstract class ImapObjectCollection<T> : IEnumerable<T>
    {

        protected ImapClient Client;
        protected List<T> List;

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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        #endregion

    }
}
