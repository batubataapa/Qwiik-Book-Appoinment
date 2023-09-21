using BookAppoinment.Adapters.Repositories.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookAppoinment.Adapters;

public class QwiikDataContext : DbContext
{
    protected QwiikDataContext()
    {
    }

    protected QwiikDataContext(DbContextOptions options) : base(options)
    {
    }

    protected QwiikDataContext(DbContextOptions<QwiikDataContext> options) : base(options)
    {
    }

    public virtual DbSet<AgenciesDto> Agencies { get; set; }
    public virtual DbSet<AppointmentsDto> Appointments { get; set; }
    public virtual DbSet<CustomersDto> Customers { get; set; }
    public virtual DbSet<MaximumAppointmentsDto> MaximumAppointments { get; set; }
    public virtual DbSet<OffDaysDto> OffDays { get; set; }
}