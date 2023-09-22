using System;
using AutoMapper;
using BookAppoinment.Adapters.Errors;
using BookAppoinment.Adapters.Repositories.Dtos;
using BookAppoinment.Adapters.Repositories.Interfaces;
using BookAppoinment.Domain.Entities.Appointment.Command;
using BookAppoinment.Domain.Entities.Appointment.Queries;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using static BookAppoinment.Domain.Entities.Appointment.Command.CreateAppointment;

namespace BookAppoinment.Domain.UnitTests.Appointment.Command;

public class CreateAppointment_Tests : Qwiik_Tests
{
    private readonly CreateAppointment _sut;


    public CreateAppointment_Tests(ITestOutputHelper output) : base(output) =>
        _sut = new CreateAppointment(output.BuildLoggerFor<CreateAppointment>(),
            _scope.ServiceProvider.GetRequiredService<IMapper>(),
            _scope.ServiceProvider.GetRequiredService<IBookingRepository>());

    [Fact]
    public async Task Handle_CreateAppointment_Success_NewCustomer()
    {
        _dbContext.Customers.Add(new CustomersDto { Address = "123 Main Street, City 1", FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Phone = "+1234567890" });
        await _dbContext.SaveChangesAsync();

        // Let's set up the command
        var customer = new CreateAppointmentCustomerRequest { Address = "123 Main Street, City 1", FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "+1234567890" };
        var createAppointmentRequestDto = new CreateAppointmentRequestDto { Customer = customer, AppointmentDate = DateTime.MaxValue };
        var createAppointmentCommand = new CreateAppointmentCommand(1, createAppointmentRequestDto);

        var resp = await _sut.Handle(createAppointmentCommand, new CancellationToken());
        resp.Match(
            Left: _ => Assert.False(true),
            Bottom: () => Assert.False(true),
            Right: u => Assert.IsType<CreateAppointmentResponse>(u));
    }

    [Fact]
    public async Task Handle_CreateAppointment_Success_ExistingCustomer()
    {
        _dbContext.Customers.Add(new CustomersDto { Address = "123 Main Street, City 1", FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "+1234567890" });
        await _dbContext.SaveChangesAsync();

        // Let's set up the command
        var customer = new CreateAppointmentCustomerRequest { Address = "123 Main Street, City 1", FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "+1234567890" };
        var createAppointmentRequestDto = new CreateAppointmentRequestDto { Customer = customer, AppointmentDate = DateTime.MaxValue };
        var createAppointmentCommand = new CreateAppointmentCommand(1, createAppointmentRequestDto);

        var resp = await _sut.Handle(createAppointmentCommand, new CancellationToken());
        resp.Match(
            Left: _ => Assert.False(true),
            Bottom: () => Assert.False(true),
            Right: u => Assert.IsType<CreateAppointmentResponse>(u));
    }

    [Fact]
    public async Task Handle_GetAppointmentDetail_Failed_Offdays()
    {
        _dbContext.OffDays.Add(new OffDaysDto { OffDayStatus = true, AppointmentDate = DateOnly.FromDateTime(DateTime.MaxValue), AgencyId = 1 });
        await _dbContext.SaveChangesAsync();

        // Let's set up the command
        var customer = new CreateAppointmentCustomerRequest { Address = "123 Main Street, City 1", FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "+1234567890" };
        var createAppointmentRequestDto = new CreateAppointmentRequestDto { Customer = customer, AppointmentDate = DateTime.MaxValue };
        var createAppointmentCommand = new CreateAppointmentCommand(1, createAppointmentRequestDto);

        var resp = await _sut.Handle(createAppointmentCommand, new CancellationToken());
        resp.Match(
            Left: ex =>
            {
                Assert.IsType<QwiikInternalServerError>(ex);
                Assert.Equal("This agency is having off days", ex.Message);
            },
            Bottom: () => Assert.False(true),
            Right: _ => Assert.False(true));
    }

    [Fact]
    public async Task Handle_GetAppointmentDetail_Failed_MaximumReached()
    {
        var appointments = new List<AppointmentsDto>();
        for (int i = 0; i < 3; i++)
        {
            appointments.Add(new AppointmentsDto { AppointmentDate = DateOnly.MaxValue, CreatedAt = DateTime.UtcNow, CustomerId = i, AgencyId = 1, Token = Guid.NewGuid().ToString() });
        }
        _dbContext.Appointments.AddRange(appointments);
        _dbContext.MaximumAppointments.Add(new MaximumAppointmentsDto { MaximumAppointmentStatus = true, AppointmentDate = DateOnly.FromDateTime(DateTime.MaxValue), AgencyId = 1, MaximumAppointmentNumber = 3 });
        await _dbContext.SaveChangesAsync();

        // Let's set up the command
        var customer = new CreateAppointmentCustomerRequest { Address = "123 Main Street, City 1", FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "+1234567890" };
        var createAppointmentRequestDto = new CreateAppointmentRequestDto { Customer = customer, AppointmentDate = DateTime.MaxValue };
        var createAppointmentCommand = new CreateAppointmentCommand(1, createAppointmentRequestDto);

        var resp = await _sut.Handle(createAppointmentCommand, new CancellationToken());
        resp.Match(
            Left: ex =>
            {
                Assert.IsType<QwiikInternalServerError>(ex);
                Assert.Equal("Maximum appointments reached", ex.Message);
            },
            Bottom: () => Assert.False(true),
            Right: _ => Assert.False(true));
    }
}