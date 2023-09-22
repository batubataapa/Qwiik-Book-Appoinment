using Ardalis.ApiEndpoints;
using BookAppoinment.Adapters.Model;
using BookAppoinment.Domain.Entities.Appointment.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookAppoinment.Endpoints.Appointment;

public class GetAppointmentList : EndpointBaseAsync
    .WithRequest<GetAppointmentListRequest>
    .WithResult<IActionResult>
{
    protected readonly IMediator _mediator;

    public GetAppointmentList(IMediator mediator) =>
        _mediator = mediator;

    /// <summary>
    ///     Handle Lists of Appointment by AgencyId
    /// </summary>
    /// <response code="200">Return success message</response>
    [HttpGet("/appointments/{agencyId}")]
    [SwaggerOperation(Tags = new[] { "Appointments" })]
    [ProducesResponseType(typeof(QwiikResponse<AppointmentListResponse>), StatusCodes.Status200OK)]
    public override async Task<IActionResult>
        HandleAsync([FromQuery] GetAppointmentListRequest request, CancellationToken cancellationToken = new()) =>
        (await _mediator.Send(new GetAppointmentListQuery(request.AgencyId, request.StartDate, request.EndDate, request.Page, request.PerPage), cancellationToken))
                .Match<IActionResult>(
                    data => QwiikResponse<AppointmentListResponse>.CreateFrom(data),
                     QwiikResponse<AppointmentListResponse>.CreateFromError);
}

public class GetAppointmentListRequest
{
    [FromRoute(Name = "agencyId")] public int AgencyId { get; init; }
    [FromQuery(Name = "startDate")] public DateTime StartDate { get; init; } = DateTime.MinValue;
    [FromQuery(Name = "endDate")] public DateTime EndDate { get; init; } = DateTime.MaxValue;
    [FromQuery(Name = "page")] public int Page { get; init; } = 1;
    [FromQuery(Name = "perPage")] public int PerPage { get; init; } = 10;
}