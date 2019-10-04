using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;

using CustomersApi.Extensions;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace CustomersApi.Middleware
{
    public class ExceptionCaptureMiddleware
    {
        private readonly ILogger<ExceptionCaptureMiddleware> _logger;

        private readonly RequestDelegate _next;

        public ExceptionCaptureMiddleware(RequestDelegate next, ILogger<ExceptionCaptureMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SecurityException ex)
            {
                var response = context.Response;
                _logger.LogCritical(ex, "Unhandled security exception");

                if (!response.HasStarted)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            catch (ValidationException ex)
            {
                var response = context.Response;

                if (!response.HasStarted)
                {
                    var errorResponse = JsonConvert.SerializeObject(
                        new
                        {
                            Errors = ex.GroupedByProperty()
                        });
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    await response.WriteAsync(errorResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                var response = context.Response;

                // modifying a response that has started will throw an exception
                if (!response.HasStarted)
                {
                    var errorResponse = JsonConvert.SerializeObject(
                        new
                        {
                            Message = $"An unexpected error occurred: {ex.Message}"
                        });
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    await response.WriteAsync(errorResponse);
                }
            }
        }
    }
}
