using Parking_Ticket_Management_App.Controllers;
using Parking_Ticket_Management_App.Memory.Models;

namespace Parking_Ticket_Management_App.Memory
{
    public interface IMemoryAccess
    {
        void AddTicketToMemory(ParkingTicket ticket);
        List<ParkingTicket> GetAllParkingTicketsForLicensePlate(string licensePlate);
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
    }
}
