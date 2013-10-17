namespace ImapX.Authentication
{
    public interface IImapCredentials
    {

        /// <summary>
        /// Provides the authentication command to be send to the server
        /// </summary>
        /// <returns></returns>
        string ToCommand(Capability capabilities);

        bool IsMultiCommand();

        bool ProcessAnswers();

        /// <summary>
        /// Checks whether the authntication mechanism used is supported by the server
        /// </summary>
        /// <param name="capabilities"></param>
        /// <returns></returns>
        bool IsSupported(Capability capabilities);

    }
}