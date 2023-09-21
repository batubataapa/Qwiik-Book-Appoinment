using System;
namespace BookAppoinment.Adapters.Errors;

public class QwiikInternalServerError : QwiikError
{
    public QwiikInternalServerError(string message = "") : base(QwiikErrorType.QwiikInternalServerError, message)
    {
    }

    public QwiikInternalServerError(Exception ex) : base(ex)
    {
        var dotnetEnv = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var aspnetEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (dotnetEnv != "Production" && aspnetEnv != "Production")
            Message = ex.Message + "\n" + (ex.StackTrace ?? string.Empty);
    }
}