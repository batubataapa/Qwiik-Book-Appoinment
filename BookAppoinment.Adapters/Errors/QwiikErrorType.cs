using System;
using Ardalis.SmartEnum;

namespace BookAppoinment.Adapters.Errors;

public abstract class QwiikErrorType : SmartEnum<QwiikErrorType>
{
    public string Description { get; }

    public static readonly QwiikErrorType QwiikInternalServerError = new QwiikInternalServerErrorType();

    protected QwiikErrorType(string name, string description, int value) : base(name, value) => Description = description;

    private sealed class QwiikInternalServerErrorType : QwiikErrorType
    {
        public QwiikInternalServerErrorType() : base("QwiikInternalServerError", "An internal error has occurred", 21001)
        {
        }
    }
}

