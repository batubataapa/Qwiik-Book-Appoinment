using System;
using AutoMapper;
using BookAppoinment.Adapters.Repositories.Dtos;
using BookAppoinment.Domain.Entities.Agencies.Queries;
using BookAppoinment.Domain.Entities.Appointment.Queries;
using static BookAppoinment.Domain.Entities.Appointment.Command.CreateAppointment;

namespace BookAppoinment.Domain.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AgenciesDto, AgencyDetailResponse>();
        CreateMap<AppointmentsDto, AppointmentDetail>()
            .ForMember(s => s.AppointmentDate, m => m.MapFrom(src => src.AppointmentDate.ToDateTime(TimeOnly.MinValue)));
        CreateMap<CreateAppointmentCustomerRequest, CustomersDto>();
    }
}