using ImapX.Collections;
using ImapX.Enums;
using ImapX.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Commands
{
    public class FolderCommand<T> : ClientImapCommand<T>
    {
        protected StatusCommand _statusCmd;

        public FolderCommand(ImapClient client, long id, Folder folder) : base(client, id, folder)
        {
        }

        public FolderCommand(ImapClient client, long id, Folder folder, string code) : base(client, id, code, folder)
        {
        }

        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            ImapToken token;

            if (responseToken.Value == "FLAGS")
            {
                var flags = new List<string>();

                io.ConsumeOpeningParenthesis();

                while ((token = io.ReadToken()).Type == TokenType.Flag || token.Type == TokenType.Atom)
                    flags.Add(token.Value);

                if (token.Type != TokenType.ClosingParenthesis)
                    throw new UnexpectedTokenException();

                Folder.Flags = new FolderFlagCollection(flags, Client, Folder);

                return true;
            }
            else if (responseToken.Value == "STATUS")
            {
                if (_statusCmd == null)
                    _statusCmd = new StatusCommand(Client, -1, Folder, FolderStatusType.None);
                else
                    _statusCmd.Folder = Folder;

                return _statusCmd.HandleSpecificUntaggedResponse(io, responseToken);
            }
            else if (responseToken.Value == "OK")
            {
                return true;
            }

            return base.HandleSpecificUntaggedResponse(io, responseToken);
        }

        public override void HandleUIdNextResponseCode(ImapParser io)
        {
            Folder.UidNext = io.ReadLong();
        }

        public override void HandleUIdValidityResponseCode(ImapParser io)
        {
            Folder.UidValidity = io.ReadLong();
        }

        public override void HandleUnseenResponseCode(ImapParser io)
        {
            Folder.FirstUnseen = io.ReadLong();
        }

        public override void HandlePermanentFlagsResponseCode(ImapParser io)
        {
            ImapToken token;
            io.ConsumeOpeningParenthesis();

            var flags = new List<string>();

            while ((token = io.ReadToken()).Type == TokenType.Flag || token.Type == TokenType.Atom)
                flags.Add(token.Value);

            if (token.Type != TokenType.ClosingParenthesis)
                throw new UnexpectedTokenException();

            Folder.AllowedPermanentFlags = flags;
        }

        public override void HandleReadOnlyResponseCode(ImapParser io)
        {
            Folder.ReadOnly = true;
        }

        public override void HandleReadWriteResponseCode(ImapParser io)
        {
            Folder.ReadOnly = false;
        }

        public override void HandleHighestModSeqResponseCode(ImapParser io)
        {
            Folder.HighestModSeq = io.ReadLong();
        }

    }

    public class FolderCommand : FolderCommand<object>
    {
        public FolderCommand(ImapClient client, long id, Folder folder) : base(client, id, folder)
        {
        }

        public FolderCommand(ImapClient client, long id, Folder folder, string code) : base(client, id, folder, code)
        {
        }
    }
}
