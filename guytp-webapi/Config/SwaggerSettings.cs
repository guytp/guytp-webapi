namespace Guytp.WebApi
{
    /// <summary>
    /// This configuration element defines core API configuration.
    /// </summary>
    public class SwaggerSettings
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether or not Swagger is enabled for this API.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets whether to override the root URL for document requests witha  predefined value.
        /// </summary>
        public string OverrideRootUrl { get; set; }

        /// <summary>
        /// Gets or sets whether or not Swagger UI is enabled for this API.
        /// </summary>
        public bool IsUiEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Swagger docs route, relative to the API's base.
        /// </summary>
        public string SwaggerRoute { get; set; }

        /// <summary>
        /// Gets or sets the Swagger UI route, relative to the API's base.
        /// </summary>
        public string SwaggerUiRoute { get; set; }

        public string Host { get; set; }

        public string BasePath { get; set; }

        public string Scheme { get; set; }
        #endregion

        public SwaggerSettings()
        {
            IsEnabled = true;
            IsUiEnabled = true;
            SwaggerRoute = "_md/docs";
            SwaggerUiRoute = "_md";
            //Host = "localhost";
            //BasePath = "/";
            //Scheme = "https";
        }
    }
}