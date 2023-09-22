using Ardalis.ApiEndpoints;
using BookAppoinment.Adapters.Model;
using BookAppoinment.Domain.Entities.Agencies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookAppoinment.Endpoints.Agencies;

public class GetAgencyDetail : EndpointBaseAsync
    .WithRequest<GetAgencyDetailRequest>
    .WithResult<IActionResult>
{
    protected readonly IMediator _mediator;

    public GetAgencyDetail(IMediator mediator) =>
        _mediator = mediator;

    /// <summary>
    ///     Handle Agency detail
    /// </summary>
    /// <response code="200">Return success message</response>
    [HttpGet("/agencies/{agencyId}")]
    [SwaggerOperation(Tags = new[] { "Agencies" })]
    [ProducesResponseType(typeof(QwiikResponse<AgencyDetailResponse>), StatusCodes.Status200OK)]
    public override async Task<IActionResult>
        HandleAsync([FromRoute] GetAgencyDetailRequest request, CancellationToken cancellationToken = new())
    {
        var test = (await _mediator.Send(new GetAgencyDetailQuery(request.AgencyId), cancellationToken))
                .Match<IActionResult>(
                    data => QwiikResponse<AgencyDetailResponse>.CreateFrom(data),
                     QwiikResponse<AgencyDetailResponse>.CreateFromError);
        return test;
    }
}

public class GetAgencyDetailRequest
{
    [FromRoute(Name = "agencyId")] public int AgencyId { get; init; }
}