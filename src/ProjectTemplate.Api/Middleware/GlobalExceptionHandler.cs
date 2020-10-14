using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Security.Authentication;
using ProjectTemplate.Domain.Helpers;
using Serilog;
using ILogger = Serilog.ILogger;
using ProjectTemplate.Domain.Exceptions;
using ProjectTemplate.Application;

namespace ProjectTemplate.Api.Middleware
{
    public static class GlobalExceptionHandler
    {
        private static ILogger _logger;

        private static bool _sendErrorDetails;

        public static void HandleExceptions(
            this IApplicationBuilder builder, 
            ILogger logger,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _sendErrorDetails = env.IsDevelopment();
            builder.Run(HandleServerError);
        }

        private static async Task HandleServerError(HttpContext context)
        {
            var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (error == null)
            {
                return;
            }

            var accessControlOriginKey = "Access-Control-Allow-Origin";
            (var errorMessage, var statusCode) = GetErrorMessageAndStatusCode(error);
            var response = new ErrorResponse
            {
                Message = errorMessage,
            };

            _logger.Error(error, error.Message);

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)statusCode;

            if (!context.Response.Headers.ContainsKey(accessControlOriginKey))
            {
                context.Response.Headers.Append(accessControlOriginKey, "*");
            }

            if (_sendErrorDetails)
            {
                response.Data = error.Data;
                response.StackTrace = error.StackTrace;
                response.InnerExceptionMessage = error.InnerException != null ? error.GetBaseException().Message : null;
            }

            else if (error is ValidationException)
            {
                response.Data = error.Data;
            }

            await context.Response.WriteAsync(response.ToJson(true)).ConfigureAwait(false);
        }

        private static (string, HttpStatusCode) GetErrorMessageAndStatusCode(Exception exception)
        {
            var statusCode = default(HttpStatusCode);
            var defaultMsg = default(string);

            switch (exception)
            {
                case KeyNotFoundException kException:
                    statusCode = HttpStatusCode.NotFound;
                    defaultMsg = "The requested resource cannot be found";
                    break;

                case ArgumentException aExcption:
                case ValidationException vException:
                    statusCode = HttpStatusCode.BadRequest;
                    defaultMsg = "Invalid request";
                    break;

                case AccessViolationException avException:
                case UnauthorizedAccessException uaExption:
                    statusCode = HttpStatusCode.Unauthorized;
                    defaultMsg = "Access denied";
                    break;

                case NotImplementedException niException:
                    statusCode = HttpStatusCode.NotImplemented;
                    defaultMsg = "The requested resource is currently not available";
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    defaultMsg = "Internal Server Error";
                    break;
            }

            return (GetErrorMessage(exception, defaultMsg), statusCode);
        }

        private static string GetErrorMessage(Exception exception, string defaultMessage)
        {
            if (exception is ValidationException)
            {
                return exception.Message;
            }
            
            var errorMsg = default(string);

            if (exception is AppException appException)
            {
                errorMsg = appException.FriendlyMessage ?? appException.Message;
            }

            return errorMsg ?? (_sendErrorDetails ? exception.Message : defaultMessage);
        }
    }
}

