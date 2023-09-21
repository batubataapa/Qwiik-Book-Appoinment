using System;
namespace BookAppoinment.Adapters.Errors;

public abstract class QwiikError
{
    public QwiikErrorType ErrorType { get; init; }
    public string Message { get; init; }

    protected QwiikError(QwiikErrorType errorType, string message = "")
    {
        ErrorType = errorType;
        Message = string.IsNullOrWhiteSpace(message)
            ? errorType.Description
            : message;
    }

    protected QwiikError(Exception ex) : this(QwiikErrorType.QwiikInternalServerError, ex.Message)
    {
    }
}