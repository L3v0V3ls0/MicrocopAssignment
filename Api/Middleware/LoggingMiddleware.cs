using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(
            RequestDelegate next,
            ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            Exception? exception = null;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                // Always log in finally block
                await LogRequestAsync(context, startTime, exception);
            }
        }

        private async Task LogRequestAsync(HttpContext context, DateTime startTime, Exception? exception)
        {
                // Get all required fields from the assignment
            var logEntry = new
            {
                LogLevel = exception != null ? "Error" : "Info",         
                Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ClientIP = GetClientIp(context),                        
                ClientName = context.Items["ClientName"] as string ?? "Unknown",
                HostName = Environment.MachineName,
                MethodName = $"{context.Request.Method} {context.Request.Path}", 
                Parameters = GetRequestParameters(context),
                Message = exception != null ?
                    $"Error: {exception.Message}" : "Request completed",
                StatusCode = context.Response.StatusCode,
                UserAgent = context.Request.Headers.UserAgent.ToString()
            };

            // Write to x log file (JSON format as per assignment)
            await WriteToDailyLogFile(logEntry);
        }

        private string GetClientIp(HttpContext context)
        {
            // Check for forwarded headers (behind proxy/load balancer)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
                return forwardedFor.Split(',')[0].Trim();

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
        
        private string GetRequestParameters(HttpContext context)
        {
            var parameters = new List<string>();

            // Add query string parameters
            if (context.Request.QueryString.HasValue)
                parameters.Add($"Query: {context.Request.QueryString}");

            // Try to read body for POST/PUT requests
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                if (context.Request.ContentLength > 0 &&
                    context.Request.ContentType?.Contains("application/json") == true)
                {
                    parameters.Add("Body: [JSON payload]");
                }
            }

            return string.Join("; ", parameters);
        }
        //Creates /Logs dir and writes json seriealized log entry in a daily new file
        private async Task WriteToDailyLogFile(object logEntry)
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);

            var logDate = DateTime.UtcNow;
            var logFileName = $"Log_{logDate:yyyyMMdd}.log";
            var logFilePath = Path.Combine(logDirectory, logFileName);

            var logLine = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions
            {
                WriteIndented = false
            });

            await File.AppendAllTextAsync(logFilePath, logLine + Environment.NewLine);
        }

    }
}
