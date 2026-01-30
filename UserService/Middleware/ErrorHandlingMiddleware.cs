using System.Net;
using UserService.Application.Models.Error;

namespace UserService.WebApi.Middleware
{
    /// <summary>
    /// Error handling to give correct response
    /// </summary>
    /// <param name="_next"></param>
    public sealed class ErrorHandlingMiddleware(RequestDelegate _next, ILogger<ErrorHandlingMiddleware> logger)
    {
        /// <summary>
        ///single method to catch the exceptions and return accordingly
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handle excpetion properly
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            logger.LogError("$Unhandled exception occured while processing the request");
            httpContext.Response.StatusCode = ex switch
            {
                UserNotFoundException => (int)HttpStatusCode.NotFound,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,
            };
            if (!httpContext.Response.HasStarted)
            {
                var payload = new { error = ex.Message };
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(payload);
            }
        }
    }
}
