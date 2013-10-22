using System;
using System.Collections.Generic;
using System.Linq;
using ImapX.Constants;

namespace ImapX.Collections
{
    public class FolderFlagCollection : ImapObjectCollection<string>
    {
        readonly Folder _folder;

        public FolderFlagCollection(ImapClient client, Folder folder)
            : base(client)
        {
            _folder = folder;
        }

        public FolderFlagCollection(IEnumerable<string> items, ImapClient client, Folder folder)
            : base(client, items)
        {
            _folder = folder;
        }

        /// <summary>
        /// Adds a folder flag
        /// </summary>
        /// <param name="flag">The flag to be added</param>
        /// <returns><code>true</code> if the flag could be added</returns>
        /// <exception cref="System.ArgumentException">If the flag is empty</exception>
        public bool Add(string flag)
        {
            if (string.IsNullOrEmpty(flag))
                throw new ArgumentException("Flag cannot be empty");

            return AddRange(new[] { flag });
        }

        /// <summary>
        /// Adds a list of specified folder flags
        /// </summary>
        /// <param name="flags">The flags to be added</param>
        /// <returns><code>true</code> if the flags could be added</returns>
        public bool AddRange(IEnumerable<string> flags)
        {
            if (!Client.Capabilities.Metadata || _folder.AllowedPermanentFlags == null || !_folder.AllowedPermanentFlags.Intersect(flags).Any())
                return false;

            IList<string> data = new List<string>();
            if (Client.SendAndReceive(string.Format(ImapCommands.SetMetaData, _folder.Path,
                                                                                Client.Behavior.SpecialUseMetadataPath,
                                                                                    string.Join(" ", _folder.Flags.Concat(flags.Where(_ => !string.IsNullOrEmpty(_))).Distinct().ToArray())), ref data))
            {
                AddRangeInternal(flags.Except(List));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a folder flag
        /// </summary>
        /// <param name="flag">The flag to be removed</param>
        /// <returns><code>true</code> if the flag could be removed</returns>
        /// <exception cref="System.ArgumentException">If the flag is empty</exception>
        public bool Remove(string flag)
        {
            if (string.IsNullOrEmpty(flag))
                throw new ArgumentException("Flag cannot be empty");

            return RemoveRange(new[] { flag });
        }

        /// <summary>
        /// Removes a list of specified folder flags
        /// </summary>
        /// <param name="index">The index of the first flag to be removed</param>
        /// <param name="count">The number of flags to be removed</param>
        /// <returns><code>true</code> if the flags could be removed</returns>
        public bool RemoveRange(int index, int count)
        {
            return RemoveRange(List.Skip(index).Take(count));
        }

        /// <summary>
        /// Removes a list of specified folder flags
        /// </summary>
        /// <param name="flags">The flags to be removed</param>
        /// <returns><code>true</code> if the flags could be removed</returns>
        public bool RemoveRange(IEnumerable<string> flags)
        {
            if (!Client.Capabilities.Metadata || _folder.AllowedPermanentFlags == null || !_folder.AllowedPermanentFlags.Intersect(flags).Any())
                return false;

            IList<string> data = new List<string>();
            if (
                !Client.SendAndReceive(
                    string.Format(ImapCommands.SetMetaData, _folder.Path, Client.Behavior.SpecialUseMetadataPath,
                        string.Join(" ", _folder.Flags.Except(flags.Where(_ => !string.IsNullOrEmpty(_))).ToArray())), ref data))
                return false;

            foreach (var flag in flags)
                List.Remove(flag);

            return true;
        }
    }
}