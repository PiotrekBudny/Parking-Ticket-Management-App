using Parking_Ticket_Management_App.Memory;
using Parking_Ticket_Management_App.Memory.Models;

namespace Parking_Ticket_Management_App.Logic
{
    public interface IValidateTicketLogicHandler
    {
        ParkingTicket? ValidateTicket(List<ParkingTicket> ticketsForLicensePlate);
    }

    public class ValidateTicketLogicHandler : IValidateTicketLogicHandler
    {
        private IMemoryAccess _memoryHandler;

        public ValidateTicketLogicHandler(IMemoryAccess memoryHandler)
        {
            _memoryHandler = memoryHandler;
        }

        public ParkingTicket? ValidateTicket(List<ParkingTicket> ticketsForLicensePlate)
        {
            var currentTime = DateTime.UtcNow;

            return ticketsForLicensePlate.FirstOrDefault(ticket => ticket.ValidFrom <= currentTime && ticket.ValidTo >= currentTime);
        }
    }
}
