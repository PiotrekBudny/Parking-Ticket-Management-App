using Parking_Ticket_Management_App.Memory.Models;
using Parking_Ticket_Management_App.Utils;

namespace Parking_Ticket_Management_App.Logic
{
    public interface IUseTicketLogicHandler
    {
        ParkingTicket MarkTicketEntityAsUsed(ParkingTicket parkingTicket);
    }


    public class UseTicketLogicHandler : IUseTicketLogicHandler
    {
        ISystemDateTimeProvider _systemDateTimeProvider;

        public UseTicketLogicHandler(ISystemDateTimeProvider systemDateTimeProvider)
        {
            _systemDateTimeProvider = systemDateTimeProvider;
        }

        public ParkingTicket MarkTicketEntityAsUsed(ParkingTicket parkingTicket)
        {
            if (parkingTicket.WasUsed)
            {
                throw new InvalidOperationException($"Ticket with ID {parkingTicket.TicketId} has already been used.");
            }
            
            var currentDateTime = _systemDateTimeProvider.UtcNow;

            if (currentDateTime < parkingTicket.ValidFrom || currentDateTime > parkingTicket.ValidTo)
            {
                throw new InvalidOperationException($"Ticket with ID {parkingTicket.TicketId} is not valid at the current time.");
            }

            parkingTicket.WasUsed = true;

            return parkingTicket;
        }
    }
}
