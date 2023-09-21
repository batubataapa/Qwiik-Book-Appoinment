using System;
namespace BookAppoinment.Model;

public class QwiikMetadata
{
    public bool IsSuccess { get; init; }
    public long Timestamp { get; init; }
    public string? Description { get; set; }

    public QwiikMetadata(bool isSuccess = true)
    {
        IsSuccess = isSuccess;
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
