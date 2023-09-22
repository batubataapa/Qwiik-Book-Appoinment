using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BookAppoinment.Adapters.Errors;
using BookAppoinment.Adapters.Repositories.Dtos;
using BookAppoinment.Adapters.Repositories.Interfaces;
using BookAppoinment.Domain.Entities.Appointment.Queries;
using LanguageExt;
using MediatR;
using static BookAppoinment.Domain.Entities.Appointment.Command.CreateAppointment;
using static LanguageExt.Prelude;

namespace BookAppoinment.Domain.Entities.Appointment.Command;

public class CreateAppointment :
    IRequestHandler<CreateAppointmentCommand, Either<QwiikError, CreateAppointmentResponse>>
{
    private readonly ILogger<CreateAppointment> _log;
    private readonly IMapper _mapper;
    private readonly IBookingRepository _repository;

    public CreateAppointment(ILogger<CreateAppointment> log, IMapper mapper, IBookingRepository repository)
    {
        _log = log;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Either<QwiikError, CreateAppointmentResponse>> Handle(CreateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var customers = await _repository.GetCustomerByEmailAsync(request.Model.Customer.Email!)
            .MatchAsync(cust => cust,
                async () => await _repository.AddNewCustomerAsync(_mapper.Map<CustomersDto>(request.Model.Customer)));
        if (await _repository.CheckMaximumAppointmentsByDateAsync(DateOnly.FromDateTime(request.Model.AppointmentDate)))
            return new QwiikInternalServerError("Maximum appointments reached");
        if (await _repository.CheckOffDaysByDateAsync(DateOnly.FromDateTime(request.Model.AppointmentDate)))
            return new QwiikInternalServerError("This agency is having off days");
        return (await customers.BindAsync(async cust =>
                await _repository.AddNewAppointmentAsync(new AppointmentsDto
                { AgencyId = request.AgencyId, CustomerId = cust.Id, AppointmentDate = DateOnly.FromDateTime(request.Model.AppointmentDate) })))
            .Map(_ => new CreateAppointmentResponse()); ;
    }

    public record CreateAppointmentCommand(int AgencyId, CreateAppointmentRequestDto Model) : IRequest<Either<QwiikError, CreateAppointmentResponse>>;

    public class CreateAppointmentResponse
    {
    }

    public class CreateAppointmentRequestDto
    {
        [Required] public CreateAppointmentCustomerRequest Customer { get; set; } = new();
        [Required] public DateTime AppointmentDate { get; set; }
    }

    public class CreateAppointmentCustomerRequest
    {
        [Required] public string? FirstName { get; set; }
        [Required] public string? LastName { get; set; }
        [Required] public string? Address { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? Phone { get; set; }

    }
}