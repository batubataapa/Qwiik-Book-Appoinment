using Ardalis.ApiEndpoints;
using BookAppoinment.Adapters.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BookAppoinment.Domain.Entities.Appointment.Command.CreateAppointment;

namespace BookAppoinment.Endpoints.Appointment;

public class CreateAppointment : EndpointBaseAsync
    .WithRequest<CreateAppointmentRequest>
    .WithResult<IActionResult>
{
    protected readonly IMediator _mediator;

    public CreateAppointment(IMediator mediator) =>
        _mediator = mediator;

    /// <summary>
    ///     Handle Create new appointment for any agency
    /// </summary>
    /// <response code="200">Return success message</response>
    [HttpPost("/appointments/{agencyId}")]
    [SwaggerOperation(Tags = new[] { "Appointments" })]
    [ProducesResponseType(typeof(QwiikResponse<CreateAppointmentResponse>), StatusCodes.Status200OK)]
    public override async Task<IActionResult>
        HandleAsync([FromRoute] CreateAppointmentRequest request, CancellationToken cancellationToken = new())
    {
        var test = (await _mediator.Send(new CreateAppointmentCommand(request.AgencyId, request.Model), cancellationToken))
                .Match<IActionResult>(
                    data => QwiikResponse<CreateAppointmentResponse>.CreateFrom(data),
                     QwiikResponse<CreateAppointmentResponse>.CreateFromError);
        return test;
    }
}

public class CreateAppointmentRequest
{
    [FromRoute(Name = "agencyId")] public int AgencyId { get; set; }
    [FromBody] public CreateAppointmentRequestDto Model { get; init; } = new();
}