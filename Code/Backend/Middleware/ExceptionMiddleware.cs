using System.Net;
using System.Text.Json;
using Backend.Exceptions;
using Microsoft.AspNetCore.Hosting;

namespace Backend.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteError(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await WriteError(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (BadRequestException ex)
            {
                await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                // ✅ Show real exception only in Development
                if (_env.IsDevelopment())
                {
                    await WriteError(context, HttpStatusCode.InternalServerError, ex.ToString());
                }
                else
                {
                    await WriteError(context, HttpStatusCode.InternalServerError,
                        "An unexpected error occurred");
                }
            }
        }

        private static async Task WriteError(
            HttpContext context,
            HttpStatusCode status,
            string message)
        {
            // If headers already sent, avoid crashing middleware
            if (context.Response.HasStarted) return;

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = context.Response.StatusCode,
                error = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
