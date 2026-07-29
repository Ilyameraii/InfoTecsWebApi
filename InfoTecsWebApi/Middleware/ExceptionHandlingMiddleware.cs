using Services.Contracts.Exceptions;

namespace InfoTecsWebApi.Middleware;

/// <summary>
/// Middleware, перехватывающий необработанные исключения в конвейере запроса
/// и преобразующий их в корректный HTTP-ответ с JSON-телом ошибки.
/// Ошибки валидации CSV (<see cref="CsvValidationException"/>) возвращаются как 400 Bad Request
/// со списком сообщений, все прочие необработанные исключения — как 500 Internal Server Error.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Выполняет следующий делегат конвейера, перехватывая и обрабатывая исключения.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CsvValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { errors = ex.Errors });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}