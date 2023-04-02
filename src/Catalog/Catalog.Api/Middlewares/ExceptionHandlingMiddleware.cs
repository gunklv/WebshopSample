using Catalog.Api.Exceptions;
using Catalog.Application.Shared.Exceptions.Abstraction;

namespace Catalog.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            switch (exception)
            {
                case InvalidRequestException invalidRequestException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(
                        new
                        {
                            title = "One or more validation errors occured.",
                            status = StatusCodes.Status400BadRequest,
                            errors = invalidRequestException.Errors
                        }
                    );
                    return;
                case NotFoundException notFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync(notFoundException.Message);
                    return;
                case InvalidActionException invalidActionException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(invalidActionException.Message);
                    return;
                default:
                    _logger.LogError(exception, exception.Message);
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Internal server error");
                    return;
            };
        }
    }
}
