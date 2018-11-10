using System.Security.Cryptography.X509Certificates;

namespace Guytp.WebApi
{
    /// <summary>
    /// This configuration element allows the JWT authentication to be specified and configured for the application.
    /// </summary>
    public class JwtAuthSettings
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether or not the JWT Middleware is added to the pipeline (not required for issuing only).
        /// </summary>
        public bool IsMiddlewareEnabled { get; set; }

        /// <summary>
        /// Gets or sets the issuer (either expected or to use to issue with depending on mode of oepration) for JWTs.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience (either expected or to use to issue with depending on mode of operation) for JWTs.
        /// </summary>
        public string Audience { get; set; }

        public string CertificateData { get; set; }

        /// <summary>
        /// Gets or sets the distinguished name to use for the certificate in JWT signing.
        /// </summary>
        public string CertificateDistinguishedName { get; set; }

        /// <summary>
        /// Gets or sets the store location for the certificate to use for JWT.
        /// </summary>
        public StoreLocation CertificateStoreLocation { get; set; }

        #endregion

        public JwtAuthSettings()
        {
            IsMiddlewareEnabled = true;
            CertificateStoreLocation = StoreLocation.LocalMachine;
        }
    }
}