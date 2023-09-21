using System;
using LanguageExt;

namespace BookAppoinment.Adapters.Errors;

public static class ErrorExtensions
{
    public static Either<QwiikError, TOpt> ErrorIfNone<TOpt>(this Option<TOpt> option, QwiikError value) =>
        option.Match<Either<QwiikError, TOpt>>(v => v, () => value);
}

