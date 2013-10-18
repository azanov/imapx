using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Collections
{
    public class GMailMessageLabelCollection : MessageFlagCollection
    {

        public GMailMessageLabelCollection() {}

        public GMailMessageLabelCollection(Message message) : base(message)
        {
            
        }

        public GMailMessageLabelCollection(ImapClient client, Message message)
            : base(client, message)
        {
            AddType = "+X-GM-LABELS";
            RemoveType = "-X-GM-LABELS";
            IsUTF7 = true;
            AddQuotes = true;
        }

        /// <summary>
        ///     Adds a label to the message
        /// </summary>
        /// <param name="label">The label to be added</param>
        /// <returns><code>true</code> if the label could be added</returns>
        /// <exception cref="System.ArgumentException">If the label is empty</exception>
        /// <exception cref="System.NotSupportedException">If Google Mail labels are not supported on this server</exception>
        public new bool Add(string label)
        {
            if (!Client.Capabilities.XGMExt1)
                throw new NotSupportedException("Google Mail labels are not supported on this server!");
            return base.Add(label);
        }

        /// <summary>
        ///     Adds the given labels to the message
        /// </summary>
        /// <param name="labels">The labels to be added</param>
        /// <returns><code>true</code> if the labels could be added</returns>
        /// <exception cref="System.NotSupportedException">If Google Mail labels are not supported on this server</exception>
        public new bool AddRange(IEnumerable<string> labels)
        {
            if (!Client.Capabilities.XGMExt1)
                throw new NotSupportedException("Google Mail labels are not supported on this server!");
            return base.AddRange(labels);
        }

        /// <summary>
        ///     Removes a label from a message
        /// </summary>
        /// <param name="label">The label to be removed</param>
        /// <returns><code>true</code> if the label could be removed</returns>
        /// <exception cref="System.ArgumentException">If the label is empty</exception>
        /// <exception cref="System.NotSupportedException">If Google Mail labels are not supported on this server</exception>
        public new bool Remove(string label)
        {
            if (!Client.Capabilities.XGMExt1)
                throw new NotSupportedException("Google Mail labels are not supported on this server!");

            return base.Remove(label);
        }

        /// <summary>
        ///     Removes a list of specified message labels
        /// </summary>
        /// <param name="index">The index of the first label to be removed</param>
        /// <param name="count">The number of labels to be removed</param>
        /// <returns><code>true</code> if the labels could be removed</returns>
        /// <exception cref="System.NotSupportedException">If Google Mail labels are not supported on this server</exception>
        public new bool RemoveRange(int index, int count)
        {
            if (!Client.Capabilities.XGMExt1)
                throw new NotSupportedException("Google Mail labels are not supported on this server!");
            return base.RemoveRange(index, count);
        }

        /// <summary>
        ///     Removes a list of specified message labels
        /// </summary>
        /// <param name="labels">The labels to be removed</param>
        /// <returns><code>true</code> if the labels could be removed</returns>
        /// <exception cref="System.NotSupportedException">If Google Mail labels are not supported on this server</exception>
        public new bool RemoveRange(IEnumerable<string> labels)
        {
            if (!Client.Capabilities.XGMExt1)
                throw new NotSupportedException("Google Mail labels are not supported on this server!");
            return base.RemoveRange(labels);
        }
    }
}