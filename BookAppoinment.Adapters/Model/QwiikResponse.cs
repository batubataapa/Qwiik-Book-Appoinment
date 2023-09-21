using System;
using System.Net;
using BookAppoinment.Adapters.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BookAppoinment.Adapters.Model;

public class QwiikResponse<T>: IActionResult
{

    public QwiikMetadata Message { get; init; }
    public T? Data { get; init; }
    internal HttpStatusCode ResponseCode { get; init; }

    protected QwiikResponse(T data, HttpStatusCode responseCode)
    {
        Data = data;
        Message = new QwiikMetadata();
        ResponseCode = responseCode;
    }

    protected QwiikResponse(HttpStatusCode responseCode)
    {
        Data = default;
        Message = new QwiikMetadata();
        ResponseCode = responseCode;
    }

    public virtual async Task ExecuteResultAsync(ActionContext context)
    {
        // We know we have an error, so let's tag in the correlationId
        var objectResult = new ObjectResult(this) { StatusCode = (int)ResponseCode };
        await objectResult.ExecuteResultAsync(context);
    }

    public static QwiikResponse<T> CreateFrom(T data, HttpStatusCode responseCode = HttpStatusCode.OK) =>
        new(data, responseCode);

    public static QwiikResponse<T> CreateFromError(QwiikError data)
    {
        // We do this because we might be casting data to MantraError, and 
        // for some reason type-based pattern matching doesn't work polymorphically
        var responseCode = data.GetType().Name switch
        {
            // InternalServerError 500
            nameof(QwiikInternalServerError) => HttpStatusCode.InternalServerError,

            _ => throw new NotImplementedException(
                $"Mapping to response code for {data.GetType().Name} has not been defined")
        };

        return new(responseCode)
        {
            Message = new ErrorMetadata(
                data.ErrorType.Value.ToString("000000"),
                data.ErrorType.Name,
                data.Message)
        };
    }
}

