using Parking_Ticket_Management_App.Controllers;
using Parking_Ticket_Management_App.Memory.Models;

namespace Parking_Ticket_Management_App.Memory
{
    public interface IMemoryAccess
    {
        void AddTicketToMemory(ParkingTicket ticket);
        List<ParkingTicket> GetAllParkingTicketsForLicensePlate(string licensePlate);
        void UpdateTicketInMemory(ParkingTicket ticket);

        ParkingTicket? GetTicketByIdentity(Guid ticketId);
    }
    
    public class MemoryHandler : IMemoryAccess
    {
        private readonly ILogger<ParkingTicketController> _logger;

        public MemoryHandler(ILogger<ParkingTicketController> logger)
        {
            _logger = logger;
        }

        public void AddTicketToMemory(ParkingTicket ticket)
        {
            try
            {
                ParkingTicketMemory.TicketsInMemory.Add(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a ticket to memory.");
                throw;
            }
        }

        public List<ParkingTicket> GetAllParkingTicketsForLicensePlate(string licensePlate)
        {
            return [.. ParkingTicketMemory.TicketsInMemory.Where(t => t.LicensePlate.Equals(licensePlate, StringComparison.OrdinalIgnoreCase))];
        }

        public ParkingTicket? GetTicketByIdentity(Guid ticketId)
        {
            return ParkingTicketMemory.TicketsInMemory.FirstOrDefault(t => t.TicketId == ticketId);
        }

        public void UpdateTicketInMemory(ParkingTicket ticket)
        {
            var index = ParkingTicketMemory.TicketsInMemory.FindIndex(t => t.TicketId == ticket.TicketId);

            if (index >= 0) ParkingTicketMemory.TicketsInMemory[index] = ticket;
            
            else
            {
                throw new KeyNotFoundException($"Ticket with ID {ticket.TicketId} not found in memory.");
            }
        }
    }
}
