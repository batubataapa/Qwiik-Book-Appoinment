using System.Net;
using Ardalis.ApiEndpoints;
using BookAppoinment.Adapters.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookAppoinment.Endpoints.Api;

public class Info : EndpointBaseSync
    .WithoutRequest
    .WithResult<IActionResult>
{
    /// <summary>
    /// Returns api info
    /// </summary>
    /// <response code="200">Return success</response>
    [HttpGet("/api/info")]
    [SwaggerOperation(Tags = new[] { "Api" })]
    [ProducesResponseType(typeof(QwiikResponse<InfoResponse>), StatusCodes.Status200OK)]
    public override IActionResult Handle() =>
        QwiikResponse<InfoResponse>.CreateFrom(new InfoResponse(), HttpStatusCode.OK);

}

public class InfoResponse
{
    public long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public DateTime DateTimeNow => DateTime.Now;
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
    public string AspNetCoreEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
}
