using FluentAssertions;
using Parking_Ticket_Management_App.Logic;

namespace Parking_Ticket_Management_App_Tests
{
    public class PriceCalculationLogicTests
    {

        [Test]
        public void WillCalculatePriceCorrectlyForGivenValidityPeriod()
        {
            //ARRANGE
            var validFrom = new DateTime(2024, 6, 10, 10, 30, 45, DateTimeKind.Utc);
            var validTo = new DateTime(2024, 6, 20, 22, 0, 0, DateTimeKind.Utc);
            var expectedAmount = 50.00m; // 10 days * 5.00 per day
            var priceCalculationLogicHandler = new PriceCalculationLogicHandler();
            
            //ACT
            var calculatedPrice = priceCalculationLogicHandler.CalculatePrice(validFrom, validTo);
            //ASSERT
            calculatedPrice.Should().Be(expectedAmount);
        }

        [Test]
        public void WillCalculatePriceCorrectlyForWholeMonth()
        {
            //ARRANGE
            var validFrom = new DateTime(2024, 5, 31, 22, 00, 00, DateTimeKind.Utc);
            var validTo = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc);
            var expectedAmount = 150.00m; // 10 days * 5.00 per day
            var priceCalculationLogicHandler = new PriceCalculationLogicHandler();

            //ACT
            var calculatedPrice = priceCalculationLogicHandler.CalculatePrice(validFrom, validTo);
            //ASSERT
            calculatedPrice.Should().Be(expectedAmount);
        }

        [Test]
        public void WillThrowArgumentExceptionWhenNumberOfDaysIsZeroOrNegative()
        {
            //ARRANGE
            var validFrom = new DateTime(2024, 6, 20, 10, 30, 45, DateTimeKind.Utc);
            var validTo = new DateTime(2024, 6, 10, 22, 0, 0, DateTimeKind.Utc);
            var priceCalculationLogicHandler = new PriceCalculationLogicHandler();
            //ACT
            Action act = () => priceCalculationLogicHandler.CalculatePrice(validFrom, validTo);
            //ASSERT
            act.Should().Throw<ArgumentException>().WithMessage("Number of days must be greater than zero.");
        }

        [Test]
        public void WillCalculatePriceCorrectlyForWinterMonth()
        {
            //ARRANGE
            var validFrom = new DateTime(2024, 11, 30, 23, 00, 00, DateTimeKind.Utc);
            var validTo = new DateTime(2024, 12, 31, 23, 0, 0, DateTimeKind.Utc);
            var expectedAmount = 155.00m; // 31 days * 5.00 per day
            var priceCalculationLogicHandler = new PriceCalculationLogicHandler();
            //ACT
            var calculatedPrice = priceCalculationLogicHandler.CalculatePrice(validFrom, validTo);
            //ASSERT
            calculatedPrice.Should().Be(expectedAmount);
        }
    }
}
