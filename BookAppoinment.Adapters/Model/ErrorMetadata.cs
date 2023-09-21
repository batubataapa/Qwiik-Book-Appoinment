using System;
namespace BookAppoinment.Adapters.Model;

public class ErrorMetadata : QwiikMetadata
{
    public string Code { get; init; }
    public string Detail { get; init; }

    public ErrorMetadata(string code, string description, string detail) : base(false)
    {
        Description = description;
        Code = code;
        Detail = detail;
    }
}