namespace Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket
{
    public class BuyTicketResponse
    {
        public Guid TicketId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
