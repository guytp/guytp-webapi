using Microsoft.AspNetCore.Authorization;

namespace Guytp.WebApi
{
    /// <summary>
    /// This attribute extends the AuthoriseRolesAttribute to allow multiple roles in an array rather than comma separated.
    /// </summary>
    public class AuthoriseRolesAttribute : AuthorizeAttribute
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="roles">
        /// A list of the permitted rtoles.
        /// </param>
        public AuthoriseRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
        #endregion
    }
}