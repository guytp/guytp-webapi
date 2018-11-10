using Microsoft.AspNetCore.Mvc;
using System;
namespace Guytp.WebApi.TestService
{
    /// <summary>
    /// This controller tests the Web API error handling functionality.
    /// </summary>
    [Route("error")]
    [Produces("application/json")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Generates a completely unhandled exception.
        /// </summary>
        /// <response code="500">
        /// A NotImplementedException will be thrown.
        /// </response>
        [HttpGet("unhandled")]
        public void Unhandled()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Generates an error that returns a URN.
        /// </summary>
        /// <response code="500">
        /// Error of urn:test guaranteed.
        /// </response>
        [HttpGet("urn")]
        public void SpecificUrn()
        {
            throw new WebApiUrnException("urn:test");
        }

        /// <summary>
        /// Generates an error that returns a URN with nested exception.
        /// </summary>
        /// <response code="500">
        /// Error of urn:test guaranteed with an inner NotImplementedException.
        /// </response>
        [HttpGet("urn-nested")]
        public void SpecificUrnWithNested()
        {
            throw new WebApiUrnException("urn:test", "An exception was encountered", new NotImplementedException());
        }
    }
}