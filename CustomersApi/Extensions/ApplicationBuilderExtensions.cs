using CustomersApi.Middleware;

using Microsoft.AspNetCore.Builder;

namespace CustomersApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestLogMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<RequestLogMiddleware>();

        public static IApplicationBuilder UseExceptionCaptureMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionCaptureMiddleware>();
    }
}
