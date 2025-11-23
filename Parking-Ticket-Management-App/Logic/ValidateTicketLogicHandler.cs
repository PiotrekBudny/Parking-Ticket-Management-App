using Parking_Ticket_Management_App.Memory.Models;
using Parking_Ticket_Management_App.Utils;

namespace Parking_Ticket_Management_App.Logic
{
    public interface IValidateTicketLogicHandler
    {
        ParkingTicket? ValidateTicket(List<ParkingTicket> ticketsForLicensePlate);
    }

    public class ValidateTicketLogicHandler : IValidateTicketLogicHandler
    {
        private readonly ISystemDateTimeProvider _systemDateTimeProvider;
        
        public ValidateTicketLogicHandler(ISystemDateTimeProvider systemDateTimeProvider)
        {
            _systemDateTimeProvider = systemDateTimeProvider;
        }

        public ParkingTicket? ValidateTicket(List<ParkingTicket> ticketsForLicensePlate)
        {
            var currentTime = _systemDateTimeProvider.UtcNow;

            var ticketThatHaveCorrectValidityDates = ticketsForLicensePlate.Where(ticket => ticket.ValidFrom <= currentTime && ticket.ValidTo >= currentTime);

            var validAndUsedTickets = ticketThatHaveCorrectValidityDates.Where(ticket => ticket.WasUsed);
            var validAndUnusedTickets = ticketThatHaveCorrectValidityDates.Where(ticket => !ticket.WasUsed);
            

            if (validAndUsedTickets.Any()) return validAndUsedTickets.OrderBy(ticket => ticket.ValidTo).First();
            else if (validAndUnusedTickets.Any()) return validAndUnusedTickets.OrderBy(ticket => ticket.ValidTo).First();
            else return null;
        }
    }
}
