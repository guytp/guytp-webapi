using System.Linq;
using System.Reflection;

namespace Guytp.WebApi
{
    /// <summary>
    /// This configuration element defines core API configuration.
    /// </summary>
    public class ApiSettings
    {
        #region Properties
        /// <summary>
        /// Gets or sets the URI to bind the Web API to.
        /// </summary>
        public string BindingUri { get; set; }

        /// <summary>
        /// Gets or sets the version number of this API which should indicate the version of the API contract rather than assembly or product versions.
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Gets whether or not CORS checks should be configured to allow anybody to call the API.
        /// </summary>
        public bool IsCorsAllowingAll { get; set; }

        /// <summary>
        /// Gets whether or not enumerations should be serialised as strings (true) or numbers (false).
        /// </summary>
        public bool IsEnumSerialisedAsString { get; set; }
        #endregion

        public ApiSettings()
        {
            IsCorsAllowingAll = true;
            IsEnumSerialisedAsString = true;
            BindingUri = "http://+:80";
        }
    }
}