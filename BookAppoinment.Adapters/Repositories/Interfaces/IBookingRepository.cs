using System;
using BookAppoinment.Adapters.Errors;
using BookAppoinment.Adapters.Repositories.Dtos;
using LanguageExt;

namespace BookAppoinment.Adapters.Repositories.Interfaces;

public interface IBookingRepository
{
    //Queries
    Task<IEnumerable<AppointmentsDto>> GetAppointmentListByAgencyIdAsync(int agencyId, DateOnly startDate, DateOnly endDate, int page = 1, int perPage = 10);
    Task<Option<CustomersDto>> GetCustomerByEmailAsync(string Email);
    Task<bool> CheckOffDaysByDateAsync(DateOnly pickedDate);
    Task<bool> CheckMaximumAppointmentsByDateAsync(DateOnly pickedDate);

    //Commands
    Task<Either<QwiikError, AppointmentsDto>> AddNewAppointmentAsync(AppointmentsDto appointments);
    Task<Either<QwiikError, CustomersDto>> AddNewCustomerAsync(CustomersDto customers);
}

