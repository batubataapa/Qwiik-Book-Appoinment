using System;
namespace BookAppoinment.Adapters.Repositories.Dtos;

public class AppointmentsDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int AgencyId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public DateOnly AppointmentDate { get; set; } 
}

