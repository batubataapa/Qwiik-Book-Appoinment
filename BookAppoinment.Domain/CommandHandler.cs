using System;
using LanguageExt;

namespace BookAppoinment.Domain;

public abstract class CommandHandler
{
    private readonly ILogger<CommandHandler> _log;

    protected CommandHandler(ILogger<CommandHandler> log) => _log = log;

    protected Exception LogExceptionAndReturn(Exception ex)
    {
        _log.LogError(ex, "{Message}", ex.Message);
        return ex;
    }

    protected Task<Exception> LogExceptionAndReturnAsync(Exception ex)
    {
        _log.LogError(ex, "{Message}", ex.Message);
        return Task.FromResult(ex);
    }
}
