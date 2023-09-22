using AutoMapper;
using BookAppoinment.Adapters.Repositories.Dtos;
using BookAppoinment.Adapters.Repositories.Interfaces;
using BookAppoinment.Domain.Entities.Appointment.Queries;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace BookAppoinment.Domain.UnitTests.Appointment.Queries;

public class GetAppointmentList_Tests : Qwiik_Tests
{
    private readonly GetAppointmentList _sut;


    public GetAppointmentList_Tests(ITestOutputHelper output) : base(output) =>
        _sut = new GetAppointmentList(output.BuildLoggerFor<GetAppointmentList>(),
            _scope.ServiceProvider.GetRequiredService<IMapper>(),
            _scope.ServiceProvider.GetRequiredService<IBookingRepository>());

    [Fact]
    public async Task Handle_GetAppointmentList_Success_Page1()
    {
        var appointments = new List<AppointmentsDto>();
        for (int i = 0; i < 18; i++)
        {
            appointments.Add(new AppointmentsDto { AppointmentDate = DateOnly.MaxValue, CreatedAt = DateTime.UtcNow, CustomerId = i, AgencyId = 1, Token = Guid.NewGuid().ToString() });
        }
        _dbContext.Appointments.AddRange(appointments);
        await _dbContext.SaveChangesAsync();

        // Let's set up the command
        var getAppointmentListQuery = new GetAppointmentListQuery(1, DateTime.MinValue, DateTime.MaxValue, 1, 10);

        var resp = await _sut.Handle(getAppointmentListQuery, new CancellationToken());
        resp.Match(
            Left: _ => Assert.False(true),
            Bottom: () => Assert.False(true),
            Right: u => { Assert.Equal(10, u.Data.Count); });
    }

    [Fact]
    public async Task Handle_GetAppointmentList_Success_Page2()
    {
        var appointments = new List<AppointmentsDto>();
        for (int i = 0; i < 18; i++)
        {
            appointments.Add(new AppointmentsDto { AppointmentDate = DateOnly.MaxValue, CreatedAt = DateTime.UtcNow, CustomerId = i, AgencyId = 1, Token = Guid.NewGuid().ToString() });
        }
        _dbContext.Appointments.AddRange(appointments);
        await _dbContext.SaveChangesAsync();

        // Let's set up the command
        var getAppointmentListQuery = new GetAppointmentListQuery(1, DateTime.MinValue, DateTime.MaxValue, 2, 10);

        var resp = await _sut.Handle(getAppointmentListQuery, new CancellationToken());
        resp.Match(
            Left: _ => Assert.False(true),
            Bottom: () => Assert.False(true),
            Right: u => { Assert.Equal(8, u.Data.Count); });
    }
}