namespace Parking_Ticket_Management_App.Logic
{
    public interface IPriceCalculationLogicHandler
    {
        decimal CalculatePrice(DateTime validFrom, DateTime validTo);
    }

    public class PriceCalculationLogicHandler : IPriceCalculationLogicHandler
    {
        public const decimal PricePerDay = 5.00m;

        public decimal CalculatePrice(DateTime validFrom, DateTime validTo)
        {
            var numberOfDays = (validTo - validFrom).Days;

            if (numberOfDays <= 0)
            {
                throw new ArgumentException("Number of days must be greater than zero.");
            }
            return numberOfDays * PricePerDay;
        }
    }
}
