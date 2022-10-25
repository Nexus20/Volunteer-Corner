using System.Net;
using ILogger = Serilog.ILogger;

namespace Volunteer_Corner.API.Middlewares {
    public class ExceptionHandlingMiddleware {

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger logger) {
            try {
                await _next(context);
            }
            catch (Exception exception) {
                HandleException(context, exception, logger);
            }
        }

        private void HandleException(HttpContext context, Exception exception, ILogger logger) {

            logger.Error(exception, "An exception was thrown as a result of the request");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Redirect("/Error/Error500");
        }
    }
}
