using System;

namespace Guytp.WebApi
{
    /// <summary>
    /// This type of exception contains a Urn which is interpreted by the Web API and is used in the response object.
    /// </summary>
    public class WebApiUrnException : Exception
    {
        #region Properties
        /// <summary>
        /// Gets the URN that defines the cause of this exception.
        /// </summary>
        public string Urn { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="urn">
        /// The URN that defines the cause of this exception.
        /// </param>
        public WebApiUrnException(string urn)
            : base()
        {
            Urn = urn;
        }

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="urn">
        /// The URN that defines the cause of this exception.
        /// </param>
        /// <param name="message">
        /// The message to include with the exception.
        /// </param>
        public WebApiUrnException(string urn, string message)
            : base(message)
        {
            Urn = urn;
        }

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="urn">
        /// The URN that defines the cause of this exception.
        /// </param>
        /// <param name="message">
        /// The message to include with the exception.
        /// </param>
        /// <param name="innerException">
        /// The inner exception contained within this exception.
        /// </param>
        public WebApiUrnException(string urn, string message, Exception innerException)
            : base(message, innerException)
        {
            Urn = urn;
        }
        #endregion
    }
}