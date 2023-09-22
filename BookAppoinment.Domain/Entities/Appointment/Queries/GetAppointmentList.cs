using AutoMapper;
using BookAppoinment.Adapters.Errors;
using BookAppoinment.Adapters.Repositories.Interfaces;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;

namespace BookAppoinment.Domain.Entities.Appointment.Queries;

public class GetAppointmentList :
    IRequestHandler<GetAppointmentListQuery, Either<QwiikError, AppointmentListResponse>>
{
    private readonly ILogger<GetAppointmentList> _log;
    private readonly IMapper _mapper;
    private readonly IBookingRepository _repository;

    public GetAppointmentList(ILogger<GetAppointmentList> log, IMapper mapper, IBookingRepository repository)
    {
        _log = log;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Either<QwiikError, AppointmentListResponse>> Handle(GetAppointmentListQuery request,
        CancellationToken cancellationToken)
    {
        var appointmentList = await _repository.GetAppointmentListByAgencyIdAsync(request.AgencyId,
            DateOnly.FromDateTime( request.StartDate), DateOnly.FromDateTime(request.EndDate.Date), request.Page, request.PerPage);
        return Optional(new AppointmentListResponse { Data = _mapper.Map<List<AppointmentDetail>>(appointmentList) })
            .ErrorIfNone(new QwiikInternalServerError());
    }
}

public record GetAppointmentListQuery(int AgencyId, DateTime StartDate, DateTime EndDate, int Page, int PerPage) : IRequest<Either<QwiikError, AppointmentListResponse>>;

public class AppointmentListResponse
{
    public List<AppointmentDetail> Data { get; set; } = new();
}

public class AppointmentDetail
{
    public int CustomerId { get; set; }
    public int AgencyId { get; set; }
    public DateTime AppointmentDate { get; set; }
}