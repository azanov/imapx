using ImapX.Enums;
using System.Collections.Generic;
using System.Linq;
using ImapX.Exceptions;
using ImapX.Constants;
using System;

namespace ImapX.Commands
{
    public abstract class ImapCommand : ResponseHandler
    {

        private long _id = -1;

        public const string CommandPrefix = "X";

        public ImapBase Base { get; set; }

        public long Id { get; set; }

        public bool BreakAfterUntagged { get; set; }

        public CommandState State { get; set; }
        public List<string> Parts { get; set; }
        public string StateDetails { get; set; }

        public ImapCommand(ImapBase imapBase, long id)
        {
            Base = imapBase;
            Parts = new List<string>();
            Id = id;
        }

        public ImapCommand(ImapBase imapBase, long id, string code) : this(imapBase, id)
        {
            Parts.Add(string.Format("{0} {1}", CommandPrefix + id, code));
        }

        public string GetNextPart()
        {
            var result = Parts.FirstOrDefault();
            if (Parts.Any())
                Parts.RemoveAt(0);
            return result;
        }

        public bool MatchesTag(string tag)
        {
            return tag == CommandPrefix + Id;
        }

        public virtual void Continue(string serverResponse)
        {
        }

        public override void HandleTaggedResponse(ImapParser io)
        {
            var token = io.ReadAtom();

            var nextToken = io.PeekToken();

            if (nextToken.Type == TokenType.OpeningBracket)
                HandleResponseCode(io);

            StateDetails = io.ReadLine();

            if (token.Value == StatusResponse.Ok)
                State = CommandState.Ok;

            else if (token.Value == StatusResponse.No)
                State = CommandState.No;

            else if (token.Value == StatusResponse.Bad)
            {
                State = CommandState.Bad;
                throw new BadCommandException("Bad command sent to server. Details: {0}", StateDetails);
            }
        }

        public override void HandleUntaggedResponse(ImapParser io)
        {
            var token = io.ReadToken();

            if (HandleSpecificUntaggedResponse(io, token)) { }
            else
            {
                switch (token.Value)
                {
                    case StatusResponse.Ok:
                        State = CommandState.Ok;
                        break;

                    case StatusResponse.PreAuth:
                        Base.HandlePreAuthenticate();
                        break;

                    case StatusResponse.Bye:
                        throw new ServerDisconnectedException();

                    default:
                        throw new UnexpectedTokenException();

                }
            }


            token = io.PeekToken();

            if (token.Type == TokenType.OpeningBracket)
                HandleResponseCode(io);
            else
                StateDetails = io.ReadLine(); // only fine on connect
        }
        
        public override void HandleResponseCode(ImapParser io)
        {
            io.ConsumeOpeningBracket();

            var token = io.ReadAtom();
            var consumeClosingBracket = true;

            switch (token.Value)
            {
                case ResponseCode.Alert:
                    io.ConsumeClosingBracket();
                    consumeClosingBracket = false;
                    Base.HandleAlert(io.ReadLine());
                    break;

                case ResponseCode.Capability:
                    Base.HandleCapabilityResponse(io); break;

                case ResponseCode.PermanentFlags:
                    HandlePermanentFlagsResponseCode(io); break;

                case ResponseCode.UIdNext:
                    HandleUIdNextResponseCode(io); break;

                case ResponseCode.UIdValidity:
                    HandleUIdValidityResponseCode(io); break;

                case ResponseCode.Unseen:
                    HandleUnseenResponseCode(io); break;

                case ResponseCode.ReadOnly:
                    HandleReadOnlyResponseCode(io); break;

                case ResponseCode.ReadWrite:
                    HandleReadWriteResponseCode(io); break;

                case ResponseCode.HighestModSeq:
                    HandleHighestModSeqResponseCode(io); break;
                    
                case ResponseCode.CompressionActive:
                case ResponseCode.AuthenticationFailed:
                    break;

                default:
#if DEBUG
                    throw new UnsupportedResponseCodeException("The response code {0} is not supported", token.Value);
#else
                    // skip unsupported response code
                    consumeClosingBracket = false;
                    io.ConsumeTillEol();
                    break;
#endif
            }

            if (consumeClosingBracket)
                io.ConsumeClosingBracket();

        }

        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            return false;
        }

        public virtual void OnCommandComplete()
        {

        }
    }

    public abstract class ClientImapCommand : ImapCommand
    {
        public ImapClient Client { get; set; }

        protected Folder _folder;

        public Folder Folder
        {
            get
            {
                return _folder ?? Client.SelectedFolder;
            }
            set
            {
                _folder = value;
            }
        }

        public ClientImapCommand(ImapClient client, long id) : base(client, id)
        {
            Client = client;
        }

        public ClientImapCommand(ImapClient client, long id, string code) : base(client, id, code)
        {
            Client = client;
        }

        public ClientImapCommand(ImapClient client, long id, Folder folder) : base(client, id)
        {
            Client = client;
            Folder = folder;
        }

        public ClientImapCommand(ImapClient client, long id, string code, Folder folder) : base(client, id, code)
        {
            Client = client;
            Folder = folder;
        }

        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Type == TokenType.Number)
            {
                var token = io.PeekToken();
                var n = int.Parse(responseToken.Value);

                switch (token.Value)
                {
                    case "EXISTS":
                        Folder.Exists = n; break;

                    case "RECENT":
                        Folder.Recent = n; break;

                    default:
                        return base.HandleSpecificUntaggedResponse(io, responseToken);
                }

                token = io.ReadToken();
                return true;
            }

            return base.HandleSpecificUntaggedResponse(io, responseToken);
        }
    }

    public abstract class ImapCommand<T> : ImapCommand
    {
        public ImapCommand(ImapBase imapBase, long id) : base(imapBase, id)
        {
        }

        public ImapCommand(ImapBase imapBase, long id, string code) : base(imapBase, id, code)
        {
        }

        public T Response { get; set; }
    }

    public abstract class ClientImapCommand<T> : ClientImapCommand
    {
        public ClientImapCommand(ImapClient client, long id) : base(client, id)
        {
        }

        public ClientImapCommand(ImapClient client, long id, string code) : base(client, id, code)
        {
        }

        public ClientImapCommand(ImapClient client, long id, Folder folder) : base(client, id, folder)
        {
        }

        public ClientImapCommand(ImapClient client, long id, string code, Folder folder) : base(client, id, code, folder)
        {
        }

        public T Response { get; set; }
    }
}
