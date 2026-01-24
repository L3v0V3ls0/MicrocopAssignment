using Core.Messages;
using Core.Services.Implementations;
using Core.Services.Interfaces;

namespace Api.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
        IApiClientService apiClientService)
        {

            // Validate API key 
            var apiKeyValidation = await ValidateApiKey(context, apiClientService);
            if (!apiKeyValidation.IsValid)
            {
                await WriteUnauthorizedResponse(context, apiKeyValidation.Message);
                return;
            }

            // Store client info for request logging middleware
            context.Items["ClientName"] = apiKeyValidation.ClientName;

            await _next(context);
        }

        //Validates api key and returns message and client name needed in logging middleware
        private async Task<(bool IsValid, string Message, string ClientName)> ValidateApiKey(HttpContext context, IApiClientService apiClientService)
        {
            // Get API key from header
            if (!context.Request.Headers.TryGetValue("X-API-Key", out var apiKeyHeader))
            {
                return (false, ApiClientMessages.ApiKeyRequired, null);
            }

            var apiKey = apiKeyHeader.ToString();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return (false, ApiClientMessages.ApiKeyRequired, null);
            }

            var apiClient = await apiClientService.GetApiClient(apiKey);

            if (apiClient == null || !apiClient.IsActive)
            {
                return (false, ApiClientMessages.InvalidApiKey, null);
            }

            return (true, null, apiClient.ClientName);
        }
        //Sets response as unauthorized
        private async Task WriteUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                Success = false,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(
                System.Text.Json.JsonSerializer.Serialize(errorResponse));
        }
    }
}
