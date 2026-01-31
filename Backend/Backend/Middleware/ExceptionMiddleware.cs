using System.Net;
using System.Text.Json;
using Backend.Exceptions;

namespace Backend.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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
            catch (Exception)
            {
                await WriteError(context, HttpStatusCode.InternalServerError,
                    "An unexpected error occurred");
            }
        }

        private static async Task WriteError(
            HttpContext context,
            HttpStatusCode status,
            string message)
        {
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
