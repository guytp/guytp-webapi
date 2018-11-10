namespace Guytp.WebApi
{
    /// <summary>
    /// This class defines information about an error state being returned to the client from the API.
    /// </summary>
    public class WebApiErrorDetails
    {
        #region Properties
        /// <summary>
        /// Gets the message defining this error which can be shown to the user.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the URN defining this error, if one is known.  If a URN is not supplied this is an unhandled exception.
        /// </summary>
        public string Urn { get; }

        /// <summary>
        /// Gets the stack trace for this error which will only be enabled if specified at the server.
        /// </summary>
        public string StackTrace { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="message">
        /// The message defining this error which can be shown to the user.
        /// </param>
        /// <param name="urn">
        /// The URN defining this error, if one is known.  If a URN is not supplied this is an unhandled exception.
        /// </param>
        /// <param name="stackTrace">
        /// The stack trace for this error which will only be enabled if specified at the server.
        /// </param>
        public WebApiErrorDetails(string message, string urn, string stackTrace)
            : base()
        {
            Message = message;
            Urn = urn;
            StackTrace = stackTrace;
        }
        #endregion
    }
}