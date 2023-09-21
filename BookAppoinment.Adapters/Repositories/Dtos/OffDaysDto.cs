using System;
namespace BookAppoinment.Adapters.Repositories.Dtos;

public class OffDaysDto
{
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public bool OffDayStatus { get; set; }
}

