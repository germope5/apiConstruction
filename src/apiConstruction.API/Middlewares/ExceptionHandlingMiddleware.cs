using apiConstruction.Application.DTOs.Responses;
using apiConstruction.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace apiConstruction.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ApiResponse<object>();

        switch (exception)
        {
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = ApiResponse<object>.ErrorResponse(exception.Message);
                break;

            case BusinessRuleException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = ApiResponse<object>.ErrorResponse(exception.Message);
                break;

            case FluentValidation.ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Errors.Select(e => e.ErrorMessage);
                errorResponse = ApiResponse<object>.ErrorResponse("Error de validación", errors);
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, "Error no manejado");
                errorResponse = ApiResponse<object>.ErrorResponse("Ocurrió un error interno del servidor");
                break;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, options);
        await response.WriteAsync(jsonResponse);
    }
}
