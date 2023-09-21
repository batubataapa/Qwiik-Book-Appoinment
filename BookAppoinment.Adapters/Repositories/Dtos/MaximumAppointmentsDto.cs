using System;
namespace BookAppoinment.Adapters.Repositories.Dtos;

public class MaximumAppointmentsDto
{
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public int MaximumAppointmentNumber { get; set; }
    public bool MaximumAppointmentStatus { get; set; }
}

