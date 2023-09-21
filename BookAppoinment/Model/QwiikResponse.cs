using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BookAppoinment.Model;

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

    public virtual async Task ExecuteResultAsync(ActionContext context)
    {
        // We know we have an error, so let's tag in the correlationId
        var objectResult = new ObjectResult(this) { StatusCode = (int)ResponseCode };
        await objectResult.ExecuteResultAsync(context);
    }

    public static QwiikResponse<T> CreateFrom(T data, HttpStatusCode responseCode = HttpStatusCode.OK) =>
        new(data, responseCode);

}

