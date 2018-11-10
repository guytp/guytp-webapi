using Guytp.Logging;

namespace Guytp.WebApi
{
    /// <summary>
    /// This configuration element allows the exception handling configuration of the Web API to be defined.
    /// </summary>
    public class ExceptionHandlingSettings
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether or not the Exception Handling Middleware is added to the pipeline.
        /// </summary>
        public bool IsMiddlewareEnabled { get; set; }

        /// <summary>
        /// Gets or sets whether to include the stack trace in outputs.  This setting applies even if the middleware is disabled and sets standard application error handling policies.
        /// </summary>
        public bool IsStackTraceIncluded { get; set; }

        /// <summary>
        /// Gets or sets whether to include the stack trace in outputs.
        /// </summary>
        public bool IsLoggingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the level to log handled exceptions at (if logging is enabled).
        /// </summary>
        public LogLevel LogLevel { get; set; }
        #endregion

        public ExceptionHandlingSettings()
        {
            LogLevel = LogLevel.Error;
            IsMiddlewareEnabled = true;
            IsLoggingEnabled = true;
#if DEBUG
            IsStackTraceIncluded = true;
#endif
        }
    }
}