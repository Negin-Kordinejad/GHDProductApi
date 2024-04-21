using FluentValidation;
using GHDProductApi.Core.Responses;
using System.Text.Json;

namespace GHDProductApi.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = new UserResponse();

            switch (exception)
            {
                case NotFoundException:
                    errorResponse.AddError(ResponseErrorCodeConstants.NotFoundException.ToString(), exception.Message);
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case ValidationException:
                    errorResponse.AddError(ResponseErrorCodeConstants.ArgumentException.ToString(), exception.Message);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                default:
                    if (exception.InnerException != null)
                    {
                        errorResponse.AddError(ResponseErrorCodeConstants.ArgumentException.ToString(), exception.InnerException.Message);
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    }
                    else
                    {
                        errorResponse.AddError(ResponseErrorCodeConstants.UnexpectedError.ToString(), " Oops! Something went wrong.");
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }

}
