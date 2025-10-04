using Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket;
using Parking_Ticket_Management_App.Extensions;
using Parking_Ticket_Management_App.Memory.Models;

namespace Parking_Ticket_Management_App.Logic
{
    public interface IBuyTicketLogicHandler
    {
        ParkingTicket BuildParkingTicketMemoryEntity(BuyTicketRequest buyTicketRequest);
    }

    public class BuyTicketLogicHandler : IBuyTicketLogicHandler
    {
        private readonly IPriceCalculationLogicHandler _priceCalculationLogicHandler;

        public BuyTicketLogicHandler(IPriceCalculationLogicHandler priceCalculationLogicHandler)
        {
            _priceCalculationLogicHandler = priceCalculationLogicHandler;
        }

        public ParkingTicket BuildParkingTicketMemoryEntity(BuyTicketRequest buyTicketRequest)
        {
            var validFrom = CalculateValidFromDate();
            var validTo = CalculateValidToDate(validFrom);

            var calculatedAmount = _priceCalculationLogicHandler.CalculatePrice(validFrom, validTo);

            return new ParkingTicket
            {
                TicketId = Guid.NewGuid(),
                LicensePlate = buyTicketRequest.LicensePlate,
                ValidFrom = CalculateValidFromDate(),
                ValidTo = validTo,
                Amount = calculatedAmount
            };
        }

        private DateTime CalculateValidFromDate() => DateTime.UtcNow.TrimMilliseconds();

        private DateTime CalculateValidToDate(DateTime validFrom)
        {
            var localValidFrom = validFrom.ToLocalTime();
            var localValidTo = new DateTime(localValidFrom.Year, localValidFrom.Month, 1, 0, 0, 0).AddMonths(1).TrimMilliseconds();

            return localValidTo.ToUniversalTime();
        }
    }
}
