using ECommerceG02.Domian.Exceptions;
using ECommerceG02.Domian.Exceptions.NotFound;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ECommerceG02.Web.CustomMiddlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && !context.Response.HasStarted)
                {
                    await HandleExceptionAsync(context, new NotFoundException("Route not found"));
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string errorCode = "APP_ERROR";
            string message = "An unexpected error occurred.";

            switch (exception)
            {
                case AppException appEx:
                    statusCode = appEx.StatusCode;
                    errorCode = appEx.ErrorCode;
                    message = appEx.Message;
                    break;
            }

            _logger.LogError(exception, "Exception caught in CustomExceptionMiddleware: {Message}, StatusCode: {StatusCode}, ErrorCode: {ErrorCode}",
                message, statusCode, errorCode);

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new
                {
                    StatusCode = statusCode,
                    ErrorCode = errorCode,
                    Message = message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
