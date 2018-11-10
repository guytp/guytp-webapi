using System;
using System.Configuration;

namespace Guytp.WebApi
{
    /// <summary>
    /// This class provides a configuration section to allow the Web API configuration to be defined.
    /// </summary>
    public class AppConfig
    {
        #region Properties
        /// <summary>
        /// Gets the application wide default configuration to use.
        /// </summary>
        public static AppConfig ApplicationInstance { get; }

        /// <summary>
        /// Gets or sets the JWT authentication settings.
        /// </summary>
        public JwtAuthSettings JwtAuth { get; set; }

        /// <summary>
        /// Gets or sets the core API settings.
        /// </summary>
        public ApiSettings Api { get; set; }


        /// <summary>
        /// Gets or sets the Swagger settings for this API.
        /// </summary>
        public SwaggerSettings Swagger { get; set; }


        /// <summary>
        /// Gets or sets the exception handling settings.
        /// </summary>
        public ExceptionHandlingSettings ExceptionHandling { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Handle one time initialisation of this class.
        /// </summary>
        static AppConfig()
        {
            AppConfig config = Config.AppConfig.ApplicationInstance.GetObject<AppConfig>("Guytp.WebApi");
            ApplicationInstance = config ?? new AppConfig();
        }

        public AppConfig()
        {
            JwtAuth = new JwtAuthSettings();
            Api = new ApiSettings();
            Swagger = new SwaggerSettings();
            ExceptionHandling = new ExceptionHandlingSettings();
        }
        #endregion
    }
}