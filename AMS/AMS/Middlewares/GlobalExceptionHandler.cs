using AMS.SHARED.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace AMS.Middlewares
{

    internal class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken token)
        {
            var source = exception.TargetSite?.DeclaringType?.FullName;
            var problemDetails = new ProblemDetails();
            problemDetails.Instance = context.Request.Path;
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }
            switch (exception)
            {
                case CustomException e:
                    context.Response.StatusCode = (int)e.StatusCode;
                        problemDetails.Detail = e.Message;

                    if (e.ErrorMessages != null && e.ErrorMessages.Any())
                    {
                        problemDetails.Extensions.Add("errors", e.ErrorMessages);
                    }
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Detail = exception.Message;
                    break;
            }
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            logger.LogError("{source}", source);
            logger.LogError("{ProblemDetail}", problemDetails.Detail);

            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                await response.WriteAsync(JsonSerializer.Serialize(problemDetails)).ConfigureAwait(false);
            }
            else
            {
                logger.LogWarning("Can't write error response. Response has already started.");
            }
            return true;

        }
    }
}

