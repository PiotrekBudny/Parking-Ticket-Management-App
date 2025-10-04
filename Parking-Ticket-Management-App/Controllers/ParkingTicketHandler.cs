using Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket;
using Parking_Ticket_Management_App.Controllers.Models.ValidateTicket;
using Parking_Ticket_Management_App.Logic;
using Parking_Ticket_Management_App.Memory;

namespace Parking_Ticket_Management_App.Controllers
{
    public interface IParkingTicketHandler
    {
        BuyTicketResponse HandleBuyTicketRequest(BuyTicketRequest buyTicketRequest);
        ValidateTicketResponse HandleValidateTicketRequest(ValidateTicketRequest validateTicketRequest);
    }

    public class ParkingTicketHandler : IParkingTicketHandler
    {
        private IBuyTicketLogicHandler _buyTicketLogicHandler;
        private IMemoryAccess _memoryAccess;
        private IValidateTicketLogicHandler _validateTicketLogicHandler;

        public ParkingTicketHandler(IBuyTicketLogicHandler buyTicketLogicHandler,
            IValidateTicketLogicHandler validateTicketLogicHandler,
            IMemoryAccess memoryAccess)
        {
            _buyTicketLogicHandler = buyTicketLogicHandler;
            _memoryAccess = memoryAccess;
            _validateTicketLogicHandler = validateTicketLogicHandler;
        }

        public BuyTicketResponse HandleBuyTicketRequest(BuyTicketRequest buyTicketRequest)
        {
            var parkingTicket = _buyTicketLogicHandler.BuildParkingTicketMemoryEntity(buyTicketRequest);
            _memoryAccess.AddTicketToMemory(parkingTicket);

            return new BuyTicketResponse
            {
                TicketId = parkingTicket.TicketId,
                Amount = parkingTicket.Amount,
                ValidFrom = parkingTicket.ValidFrom,
                ValidTo = parkingTicket.ValidTo
            };
        }

        public ValidateTicketResponse HandleValidateTicketRequest(ValidateTicketRequest validateTicketRequest)
        {
            var ticketsForLicensePlate = _memoryAccess.GetAllParkingTicketsForLicensePlate(validateTicketRequest.LicensePlate);

            var validParkingTicket = _validateTicketLogicHandler.ValidateTicket(ticketsForLicensePlate);

            if (validParkingTicket == null)
            {
                return new ValidateTicketResponse
                {
                    IsValid = false,
                    LicensePlate = validateTicketRequest.LicensePlate,
                };
            }

            return new ValidateTicketResponse
            {
                IsValid = true,
                LicensePlate = validParkingTicket.LicensePlate,
            };
        }
    }
}
