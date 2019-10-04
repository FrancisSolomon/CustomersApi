using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using CustomersApi.Extensions;
using CustomersApi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;

namespace CustomersApi.Middleware
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public RequestLogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("RequestLogger");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            request.EnableRewind();

            var logEntry = new RequestLogEntry
            {
                Url = request.Path,
                Method = request.Method,
                Timestamp = DateTime.UtcNow,
                CorrelationId = context.TraceIdentifier ?? string.Empty
            };

            // default values and do not dispose underlying stream
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                logEntry.Body = await reader.ReadToEndAsync();
            }

            request.Body.Position = 0;
            var timer = Stopwatch.StartNew();

            await _next(context);

            logEntry.DurationMs = timer.ElapsedMilliseconds;
            logEntry.StatusCode = context.Response.StatusCode;

            _logger.LogObject(LogLevel.Information, logEntry);
        }
    }
}
