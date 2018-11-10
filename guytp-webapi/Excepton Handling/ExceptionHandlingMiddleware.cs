using Guytp.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Guytp.WebApi
{
    /// <summary>
    /// The Exception Handling middleware enhances the standard error handlign capabilities within the Web API pipelines.
    /// </summary>
    internal class ExceptionHandlingMiddleware
    {
        private readonly ExceptionHandlingSettings _configuration = AppConfig.ApplicationInstance.ExceptionHandling;

        private readonly RequestDelegate _next;

        #region Constructors
        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        /// <summary>
        /// Invoke the OWIN middleware.
        /// </summary>
        /// <param name="context">
        /// The OWIN context that middleware is running in.
        /// </param>
        /// <returns>
        /// A task to indicate that middleware processing has finished.
        /// </returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (TaskCanceledException)
            {
                // Silently swallow disconnections
            }
            catch (Exception ex)
            {
                // If not enabled just re-throw as-is
                if (!_configuration.IsMiddlewareEnabled)
                    throw;

                bool rethrowOriginal = false;
                try
                {
                    // Log this if logging is enabled
                    if (_configuration.IsLoggingEnabled)
                    {
                        string logMessage = "An exception was encountered whilst processing the Web API request.";
                        switch (_configuration.LogLevel)
                        {
                            case LogLevel.Trace:
                                Logger.ApplicationInstance.Trace(logMessage, ex);
                                break;
                            case LogLevel.Debug:
                                Logger.ApplicationInstance.Debug(logMessage, ex);
                                break;
                            case LogLevel.Error:
                                Logger.ApplicationInstance.Error(logMessage, ex);
                                break;
                            case LogLevel.Info:
                                Logger.ApplicationInstance.Info(logMessage, ex);
                                break;
                            case LogLevel.Warning:
                                Logger.ApplicationInstance.Warn(logMessage, ex);
                                break;
                        }

                        // Return our custom exception type
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        bool isUrnException = ex is WebApiUrnException;
                        context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = isUrnException ? "API Exception" : "Unhandled Exception";
                        context.Response.ContentType = "application/json";
                        WebApiErrorDetails webApiErrorDetails = new WebApiErrorDetails(ex.Message, isUrnException ? ((WebApiUrnException)ex).Urn : null, _configuration.IsStackTraceIncluded ? ex.ToString() : null);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(webApiErrorDetails));
                    }
                }
                catch
                {
                    // Silently swallow failed exception handling attempts and rethrow the original exception
                    rethrowOriginal = true;
                }
                if (rethrowOriginal)
                    throw;
            }
        }
    }
}
