using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagerAPI.Middleware.Exceptions;

namespace TaskManagerAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Передаем запрос дальше в конвейере
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                var response = context.Response;
                response.ContentType = "application/problem+json";

                // Определяем HTTP статус в зависимости от типа ошибки
                var statusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    ValidationException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                response.StatusCode = statusCode;

                // Формируем стандартный JSON ответ по RFC 7807 (Problem Details)
                var problemDetails = new
                {
                    type = $"https://httpstatuses.com/{statusCode}",
                    title = ex.Message,
                    status = statusCode,
                    detail = ex.StackTrace // для dev, в продакшене можно убрать
                };

                var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions { WriteIndented = true });

                await response.WriteAsync(json);
            }
        }
    }
}
