using AMS.SHARED.Exceptions;
using AMS.SHARED.Interfaces.CurrentUser;
using AMS.SHARED.Models.ErrorReult;
using Serilog;
using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace AMS.Middlewares
{
    
        internal class ExceptionMiddleware : IMiddleware
        {
            private readonly ICurrentUser _currentUser;

            public ExceptionMiddleware(
                ICurrentUser currentUser
                )
            {
                _currentUser = currentUser;
            }

            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                try
                {
                    await next(context);
                }
                catch (Exception exception)
                {
                    string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
                    var userId = _currentUser.GetUserId();
                    if (userId != 0) LogContext.PushProperty("UserId", userId);
                    LogContext.PushProperty("UserEmail", email);
                    var source = exception.TargetSite?.DeclaringType?.FullName;
                    LogContext.PushProperty("source",source);
                    string errorId = Guid.NewGuid().ToString();
                    LogContext.PushProperty("ErrorId", errorId);
                    LogContext.PushProperty("StackTrace", exception.StackTrace);
                    var errorResult = new ErrorResult
                    {
                        Source = source,
                        Exception = exception.Message.Trim(),
                        ErrorId = errorId,
                        SupportMessage = String.Format("Provide the ErrorId {0} to the support team for further analysis.", errorId)
                    };

                    if (exception is not CustomException && exception.InnerException != null)
                    {
                        while (exception.InnerException != null)
                        {
                            exception = exception.InnerException;
                        }
                    }

                    if (exception is FluentValidation.ValidationException fluentException)
                    {
                        errorResult.Exception = "One or More Validations failed.";
                        foreach (var error in fluentException.Errors)
                        {
                            errorResult.Messages.Add(error.ErrorMessage);
                        }
                    }

                    switch (exception)
                    {
                        case CustomException e:
                            errorResult.StatusCode = (int)e.StatusCode;
                            if (e.ErrorMessages is not null)
                            {
                                errorResult.Messages = e.ErrorMessages;
                            }

                            break;

                        case KeyNotFoundException:
                            errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                            break;

                        case FluentValidation.ValidationException:
                            errorResult.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;

                        default:
                            errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }

                    Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode} and Error Id {errorId}.");
                    var response = context.Response;
                    if (!response.HasStarted)
                    {
                        response.ContentType = "application/json";
                        response.StatusCode = errorResult.StatusCode;
                        await response.WriteAsync(JsonSerializer.Serialize(errorResult));
                    }
                    else
                    {
                        Log.Warning("Can't write error response. Response has already started.");
                    }
                }
            }
        }
    }

