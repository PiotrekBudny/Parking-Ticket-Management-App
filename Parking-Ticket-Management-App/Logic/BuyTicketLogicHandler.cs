using Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket;
using Parking_Ticket_Management_App.Memory.Models;
using Parking_Ticket_Management_App.Utils;
using Parking_Ticket_Management_App.Utils.Extensions;

namespace Parking_Ticket_Management_App.Logic
{
    public interface IBuyTicketLogicHandler
    {
        ParkingTicket BuildParkingTicketMemoryEntity(BuyTicketRequest buyTicketRequest);
    }

    public class BuyTicketLogicHandler : IBuyTicketLogicHandler
    {
        private readonly IPriceCalculationLogicHandler _priceCalculationLogicHandler;
        private ISystemDateTimeProvider _systemDateTimeProvider;
        
        public BuyTicketLogicHandler(IPriceCalculationLogicHandler priceCalculationLogicHandler, ISystemDateTimeProvider systemDateTimeProvider)
        {
            _priceCalculationLogicHandler = priceCalculationLogicHandler;
            _systemDateTimeProvider = systemDateTimeProvider;
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

        private DateTime CalculateValidFromDate() => _systemDateTimeProvider.UtcNow.TrimMilliseconds();

        private DateTime CalculateValidToDate(DateTime validFrom)
        {
            var localValidFrom = validFrom.ToLocalTime();
            var localValidTo = new DateTime(localValidFrom.Year, localValidFrom.Month, 1, 0, 0, 0).AddMonths(1).TrimMilliseconds();

            return localValidTo.ToUniversalTime();
        }
    }
}
