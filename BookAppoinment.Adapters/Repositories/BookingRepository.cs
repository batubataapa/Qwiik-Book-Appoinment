using BookAppoinment.Adapters.Errors;
using BookAppoinment.Adapters.Repositories.Dtos;
using BookAppoinment.Adapters.Repositories.Interfaces;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace BookAppoinment.Adapters.Repositories;

public class BookingRepository<T> : IBookingRepository where T : QwiikDataContext
{
    private readonly QwiikDataContext _context;

    public BookingRepository(T context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppointmentsDto>> GetAppointmentListByAgencyIdAsync(int agencyId, DateOnly startDate, DateOnly endDate, int page = 1, int perPage = 10) =>
         await _context.Appointments.Where(Dtos => Dtos.AgencyId == agencyId && Dtos.AppointmentDate >= startDate || Dtos.AppointmentDate <= endDate)
            .Skip(perPage * (page - 1))
            .Take(perPage)
            .ToListAsync();

    public async Task<Option<CustomersDto>> GetCustomerByEmailAsync(string Email) =>
        Optional(await _context.Customers.Where(Dtos => Dtos.Email == Email).FirstOrDefaultAsync());

    public async Task<bool> CheckOffDaysByDateAsync(DateOnly pickedDate) =>
     await _context.OffDays.AnyAsync(Dtos => Dtos.AppointmentDate == pickedDate && Dtos.OffDayStatus);

    public async Task<bool> CheckMaximumAppointmentsByDateAsync(DateOnly pickedDate)
    {
        var max = await _context.MaximumAppointments.Where(Dtos => Dtos.AppointmentDate == pickedDate && Dtos.MaximumAppointmentStatus).FirstOrDefaultAsync();
        return await _context.Appointments.CountAsync(Dtos => Dtos.AppointmentDate == pickedDate) >= max?.MaximumAppointmentNumber;
    }

    public async Task<Either<QwiikError, AppointmentsDto>> AddNewAppointmentAsync(AppointmentsDto appointments)
    {
        try
        {
            appointments.Token = Guid.NewGuid().ToString();
            appointments.CreatedAt = DateTime.UtcNow;
            _context.Appointments.Add(appointments);
            await _context.SaveChangesAsync();
            return appointments;
        }
        catch (Exception ex)
        {
            return new QwiikInternalServerError(ex);
        }
    }

    public async Task<Either<QwiikError, CustomersDto>> AddNewCustomerAsync(CustomersDto customers)
    {
        try
        {
            _context.Customers.Add(customers);
            await _context.SaveChangesAsync();
            return customers;
        }
        catch (Exception ex)
        {
            return new QwiikInternalServerError(ex);
        }
    }
}
