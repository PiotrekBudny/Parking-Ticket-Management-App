namespace Parking_Ticket_Management_App.Memory.Models
{
    public class ParkingTicket
    {
        public Guid TicketId { get; set; }
        public string LicensePlate { get; set; } = null!;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal Amount { get; set; }
        public bool WasUsed { get; set; } 
        }
}
