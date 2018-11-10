using Microsoft.AspNetCore.Mvc;

namespace Guytp.WebApi.TestService
{
    /// <summary>
    /// This controller tests the Web API framework by echoing back a request to a user.
    /// </summary>
    [Route("echo")]
    [Produces("application/json")]
    [ApiController]
    public class EchoController : Controller
    {
        /// <summary>
        /// Returns an enumeration bac to the client.
        /// </summary>
        /// <param name="value">
        /// The enumeration value to echo back.
        /// </param>
        /// <returns>
        /// A copy of the enum passed in to the request.
        /// </returns>
        [HttpPost("{value}")]
        public TestEnum Echo([FromRoute] TestEnum value)
        {
            return value;
        }

        /// <summary>
        /// Returns the same message received.
        /// </summary>
        /// <param name="request">
        /// The request to echo back.
        /// </param>
        /// <returns>
        /// A copy of the string passed in to the request.
        /// </returns>
        [HttpPost("")]
        public string Echo([FromBody]string request)
        {
            return request;
        }

        /// <summary>
        /// Returns the same message received.
        /// </summary>
        /// <param name="request">
        /// The request to echo back.
        /// </param>
        /// <returns>
        /// A copy of the string passed in to the request.
        /// </returns>
        [HttpPost("auth")]
        [AuthoriseRoles("GlobalAdmin")]
        public string AuthEcho([FromBody]string request)
        {
            return request;
        }
    }
}