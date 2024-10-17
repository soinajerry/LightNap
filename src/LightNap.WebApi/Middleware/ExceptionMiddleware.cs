using LightNap.Core.Api;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LightNap.WebApi.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions =
            new()
            {
                Converters = { new JsonStringEnumConverter() },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        private readonly IHostEnvironment _environment = environment;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "{message}", ex.Message);

                if (ex is UnauthorizedAccessException)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                // To simplify client-side development, we convert all remaining errors to easily digestable ApiResponseDtos.

                ApiResponseDto<string> error;

                if (this._environment.IsDevelopment())
                {
                    if (string.IsNullOrWhiteSpace(ex.StackTrace))
                    {
                        error = ApiResponseDto<string>.CreateError(ex.Message);
                    }
                    else
                    {
                        error = ApiResponseDto<string>.CreateError([ex.Message, ex.StackTrace]);
                    }
                }
                else
                {
                    error = ApiResponseDto<string>.CreateError("Internal Server Error");
                }

                await ExceptionMiddleware.WriteErrorAsync(context, error);
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, ApiResponseDto<string> error)
        {
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(error, ExceptionMiddleware._jsonSerializerOptions);
            await context.Response.WriteAsync(json);
        }
    }
}