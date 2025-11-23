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
                Amount = calculatedAmount,
                WasUsed = false
            };
        }

        private DateTime CalculateValidFromDate() => _systemDateTimeProvider.UtcNow.TrimMilliseconds();

        private DateTime CalculateValidToDate(DateTime validFrom)
        {
            var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var cetValidFrom = TimeZoneInfo.ConvertTimeFromUtc(validFrom, cetTimeZone);
            var localValidTo = new DateTime(cetValidFrom.Year, cetValidFrom.Month, 1, 0, 0, 0).AddMonths(1).TrimMilliseconds();

            var utcValidTo = TimeZoneInfo.ConvertTimeToUtc(localValidTo, cetTimeZone);
            return utcValidTo;
        }
    }
}
