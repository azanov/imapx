using System;
using System.Collections.Generic;
using System.Linq;

namespace ImapX.Collections
{
    public class MessageFlagCollection : ImapObjectCollection<string>
    {
        private readonly Message _message;

        protected string AddType = "+FLAGS";
        protected string RemoveType = "-FLAGS";

        public MessageFlagCollection(ImapClient client, Message message)
            : base(client)
        {
            _message = message;
        }

        /// <summary>
        ///     Adds a flag to the message
        /// </summary>
        /// <param name="flag">The flag to be added</param>
        /// <returns><code>true</code> if the flag could be added</returns>
        /// <exception cref="System.ArgumentException">If the flag is empty</exception>
        public bool Add(string flag)
        {
            if (string.IsNullOrEmpty(flag))
                throw new ArgumentException("Flag cannot be empty");

            return AddRange(new[] {flag});
        }

        /// <summary>
        ///     Adds the given flags to the message
        /// </summary>
        /// <param name="flags">The flags to be added</param>
        /// <returns><code>true</code> if the flags could be added</returns>
        public bool AddRange(IEnumerable<string> flags)
        {
            IList<string> data = new List<string>();
            if (Client.SendAndReceive(string.Format(ImapCommands.STORE,
                _message.MessageUid, AddType,
                string.Join(" ", _message.Flags.Concat(flags.Where(_ => !string.IsNullOrEmpty(_))).Distinct())),
                ref data))
            {
                AddRangeInternal(flags.Except(List));

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Removes a flag from a message
        /// </summary>
        /// <param name="flag">The flag to be removed</param>
        /// <returns><code>true</code> if the flag could be removed</returns>
        /// <exception cref="System.ArgumentException">If the flag is empty</exception>
        public bool Remove(string flag)
        {
            if (string.IsNullOrEmpty(flag))
                throw new ArgumentException("Flag cannot be empty");

            return RemoveRange(new[] {flag});
        }

        /// <summary>
        ///     Removes a list of specified message flags
        /// </summary>
        /// <param name="index">The index of the first flag to be removed</param>
        /// <param name="count">The number of flags to be removed</param>
        /// <returns><code>true</code> if the flags could be removed</returns>
        public bool RemoveRange(int index, int count)
        {
            return RemoveRange(List.Skip(index).Take(count));
        }

        /// <summary>
        ///     Removes a list of specified message flags
        /// </summary>
        /// <param name="flags">The flags to be removed</param>
        /// <returns><code>true</code> if the flags could be removed</returns>
        public bool RemoveRange(IEnumerable<string> flags)
        {
            IList<string> data = new List<string>();
            if (
                Client.SendAndReceive(
                    string.Format(ImapCommands.STORE, _message.MessageUid, RemoveType,
                        string.Join(" ", flags.Where(_ => !string.IsNullOrEmpty(_)))), ref data))
            {
                foreach (string flag in flags)
                    List.Remove(flag);

                return true;
            }

            return false;
        }

    }
}