using System;
using System.Collections.Generic;
using System.Linq;
using ImapX.Constants;
using ImapX.EncodingHelpers;

namespace ImapX.Collections
{
    public class MessageFlagCollection : ImapObjectCollection<string>
    {
        private readonly Message _message;

        protected string AddType = "+FLAGS";
        protected string RemoveType = "-FLAGS";
        protected bool IsUTF7 = false;
        protected bool AddQuotes = false;

        public MessageFlagCollection()
        {
            
        }

        public MessageFlagCollection(Message message)
        {
            _message = message;
        }

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
            if (Client == null)
            {
                base.AddRangeInternal(flags);
                return true;
            }

            if (_message.Folder.ReadOnly)
                _message.Folder.Select();

            IList<string> data = new List<string>();
            if (!Client.SendAndReceive(string.Format(ImapCommands.Store,
                _message.UId, AddType,
                string.Join(" ",
                    this.Concat(flags.Where(_ => !string.IsNullOrEmpty(_)))
                               .Where(_ => !_.Equals(Flags.MessageFlags.Recent))
                               .Distinct()
                               .Select(_ => (AddQuotes ? "\"" : "") + _ + (AddQuotes ? "\"" : ""))
                               .Select(_ => (IsUTF7 ? ImapUTF7.Encode(_) : _)).ToArray())),
                ref data))
                return false;

            AddRangeInternal(flags.Except(List));

            return true;
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
            if (Client == null)
            {
                foreach (string flag in flags)
                    RemoveInternal(flag);
                return true;
            }

            if (_message.Folder.ReadOnly)
                _message.Folder.Select();

            IList<string> data = new List<string>();
            if (!Client.SendAndReceive(
                string.Format(ImapCommands.Store, _message.UId, RemoveType,
                    string.Join(" ",
                        flags.Where(_ => !string.IsNullOrEmpty(_))
                             .Select(_ => (AddQuotes ? "\"" : "") + _ + (AddQuotes ? "\"" : ""))
                             .Select(_ => IsUTF7 ? ImapUTF7.Encode(_) : _).ToArray())),
                ref data)) return false;
            foreach (string flag in flags)
                List.Remove(flag);

            return true;
        }

    }
}