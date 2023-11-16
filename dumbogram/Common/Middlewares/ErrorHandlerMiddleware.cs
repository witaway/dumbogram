using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dumbogram.Common.Exceptions;
using Dumbogram.Common.Helpers;

namespace Dumbogram.Common.Middlewares;

public class ErrorHandlerMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        WriteIndented = true
    };

    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(IWebHostEnvironment env, ILogger<ErrorHandlerMiddleware> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        // Properly handle special TaskCanceledException
        catch (Exception exception) when (context.RequestAborted.IsCancellationRequested)
        {
            const string message = "Request was cancelled";
            _logger.LogInformation(message);
            _logger.LogDebug(exception, message);

            context.Response.Clear();
            context.Response.StatusCode = 499; // Client Closed Request
        }
        // Handle every else exceptions
        catch (Exception exception)
        {
            exception.AddErrorCode();

            var logMessage = exception is BaseApplicationException
                ? exception.Message
                : "An unhandled exception has occurred while executing the request.";

            _logger.LogError(exception, logMessage);

            const string contentType = "application/json";
            var json = ExceptionToJson(exception);

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = contentType;
            await context.Response.WriteAsync(json);
        }
    }

    private string ExceptionToJson(Exception exception)
    {
        var exceptionMessage = exception.Message;
        var exceptionCode = exception.GetErrorCode();

        // Response contains message and code of exception
        dynamic response = new ExpandoObject();
        response.message = exceptionMessage;
        response.code = exceptionCode!;

        // And in Development environment contains ALSO full exception info and containing data
        if (_env.IsDevelopment())
        {
            var exceptionInfo = exception.ToString();
            var exceptionData = exception.Data;
            response.info = exceptionInfo;
            response.data = exceptionData;
        }

        // Try to convert response object to JSON String
        try
        {
            var serializedResponse = JsonSerializer.Serialize(
                response,
                JsonSerializerOptions
            );
            return serializedResponse;
        }
        catch (Exception ex)
        {
            const string logMessage = "An exception has occurred while serializing exception to JSON";
            _logger.LogError(ex, logMessage);
            return string.Empty;
        }
    }
}