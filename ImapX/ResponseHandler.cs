using ImapX.Exceptions;

namespace ImapX
{
    public abstract class ResponseHandler : ResponseCodeHandler
    {
        public abstract void HandleTaggedResponse(ImapParser io);

        public abstract void HandleUntaggedResponse(ImapParser io);
        
        public virtual bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            return false;
        }
        
    }

    public abstract class ResponseCodeHandler
    {
        public abstract void HandleResponseCode(ImapParser io);
        
        public virtual void HandlePermanentFlagsResponseCode(ImapParser io)
        {
            throw new TodoException();
        }

        public virtual void HandleUIdNextResponseCode(ImapParser io)
        {
            throw new TodoException();
        }

        public virtual void HandleUIdValidityResponseCode(ImapParser io)
        {
            throw new TodoException();
        }

        public virtual void HandleUnseenResponseCode(ImapParser io)
        {
            throw new TodoException();
        }

        public virtual void HandleReadOnlyResponseCode(ImapParser io)
        {
            throw new TodoException();
        }

        public virtual void HandleReadWriteResponseCode(ImapParser io)
        {
            throw new TodoException();
        }

        public virtual void HandleHighestModSeqResponseCode(ImapParser io)
        {
            throw new TodoException();
        }
    }
}
