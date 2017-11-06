using ImapX.Commands;

namespace ImapX.Authentication
{
    public abstract class ImapCredentials
    {

        /// <summary>
        /// Provides the authentication command to be send to the server
        /// </summary>
        /// <returns></returns>
        public abstract ImapCommand ToCommand(ImapBase imapBase, long id, Capability capabilities);

        /// <summary>
        /// Checks whether the authntication mechanism used is supported by the server
        /// </summary>
        /// <param name="capabilities"></param>
        /// <returns></returns>
        public abstract bool IsSupported(Capability capabilities);

    }
}