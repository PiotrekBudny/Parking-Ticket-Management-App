namespace Parking_Ticket_Management_App.Controllers.Models.ValidateTicket
{
    public class ValidateTicketResponse
    {
        public string LicensePlate { get; set; } = null!;
        public bool IsValid { get; set; }
    }
}
