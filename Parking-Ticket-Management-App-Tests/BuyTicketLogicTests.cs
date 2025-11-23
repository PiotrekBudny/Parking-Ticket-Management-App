using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;
using Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket;
using Parking_Ticket_Management_App.Logic;
using Parking_Ticket_Management_App.Memory.Models;
using Parking_Ticket_Management_App.Utils;

namespace Parking_Ticket_Management_App_Tests
{
    public class BuyTicketLogicTests
    {
        private Mock<ISystemDateTimeProvider> _systemDateTimeProviderMock;
        private Mock<IPriceCalculationLogicHandler> _priceCalculationLogicHandlerMock;

        [SetUp]
        public void Setup()
        {
            _systemDateTimeProviderMock = new Mock<ISystemDateTimeProvider>();
            _priceCalculationLogicHandlerMock = new Mock<IPriceCalculationLogicHandler>();
        }

        [Test]
        public void WillReturnParkingTicketWithValidityPeriodFromNowTillTheEndOfMonth()
        {
            //ARRANGE
            var startPeriodDateTime = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);
            var expectedEndPeriodDateTime = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc);
            var expectedAmount = 80.00m; // 16 days * 5.00 per day
            var licensePlate = "AB12345";

            _systemDateTimeProviderMock.Setup(s => s.UtcNow).Returns(startPeriodDateTime);
            _priceCalculationLogicHandlerMock.Setup(p => p.CalculatePrice(startPeriodDateTime, expectedEndPeriodDateTime)).Returns(expectedAmount);

            var buyTicketLogicHandler = new BuyTicketLogicHandler(_priceCalculationLogicHandlerMock.Object, _systemDateTimeProviderMock.Object);
            
            var buyTicketRequest = new BuyTicketRequest
            {
                LicensePlate = licensePlate
            };

            var expectedParkingTicketEntity = new ParkingTicket
            {
                TicketId = Guid.NewGuid(), // This will be validated if is in fact a Guid
                Amount = expectedAmount,
                ValidFrom = startPeriodDateTime,
                ValidTo = expectedEndPeriodDateTime,
                LicensePlate = licensePlate,
                WasUsed = false
            };

            //ACT
            var parkingTicket = buyTicketLogicHandler.BuildParkingTicketMemoryEntity(buyTicketRequest);

            //ASSERT
            parkingTicket.Should().BeEquivalentTo(expectedParkingTicketEntity, EquivalencyAssertionOptions);
        }
        
        [Test]
        public void WillReturnParkingTicketWithCorrectValidityPeriodWhenStartIsBeginingOfMonthInUtc()
        {
            //ARRANGE
            var startPeriodDateTime = new DateTime(2024, 5, 31, 22, 00, 00, DateTimeKind.Utc);
            var expectedEndPeriodDateTime = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc);
            var expectedAmount = 150.00m; // 30 days * 5.00 per day
            var licensePlate = "AB12345";

            _systemDateTimeProviderMock.Setup(s => s.UtcNow).Returns(startPeriodDateTime);
            _priceCalculationLogicHandlerMock.Setup(p => p.CalculatePrice(startPeriodDateTime, expectedEndPeriodDateTime)).Returns(expectedAmount);

            var buyTicketLogicHandler = new BuyTicketLogicHandler(_priceCalculationLogicHandlerMock.Object, _systemDateTimeProviderMock.Object);

            var buyTicketRequest = new BuyTicketRequest
            {
                LicensePlate = licensePlate
            };

            var expectedParkingTicketEntity = new ParkingTicket
            {
                TicketId = Guid.NewGuid(), // This will be validated if is in fact a Guid
                Amount = expectedAmount,
                ValidFrom = startPeriodDateTime,
                ValidTo = expectedEndPeriodDateTime,
                LicensePlate = licensePlate,
                WasUsed = false
            };

            //ACT
            var parkingTicket = buyTicketLogicHandler.BuildParkingTicketMemoryEntity(buyTicketRequest);

            //ASSERT
            parkingTicket.Should().BeEquivalentTo(expectedParkingTicketEntity, EquivalencyAssertionOptions);
        }
        
        [Test]
        public void WillReturnParkingTicketWithCorrectValidityPeriodWhenStartIsBeginingOfMonthInUtcDuringWinterTime()
        {
            //ARRANGE
            var startPeriodDateTime = new DateTime(2024, 11, 30, 23, 00, 00, DateTimeKind.Utc);
            var expectedEndPeriodDateTime = new DateTime(2024, 12, 31, 23, 0, 0, DateTimeKind.Utc);
            var expectedAmount = 155.00m; // 31 days * 5.00 per day
            var licensePlate = "AB12345";

            _systemDateTimeProviderMock.Setup(s => s.UtcNow).Returns(startPeriodDateTime);
            _priceCalculationLogicHandlerMock.Setup(p => p.CalculatePrice(startPeriodDateTime, expectedEndPeriodDateTime)).Returns(expectedAmount);

            var buyTicketLogicHandler = new BuyTicketLogicHandler(_priceCalculationLogicHandlerMock.Object, _systemDateTimeProviderMock.Object);

            var buyTicketRequest = new BuyTicketRequest
            {
                LicensePlate = licensePlate
            };

            var expectedParkingTicketEntity = new ParkingTicket
            {
                TicketId = Guid.NewGuid(), // This will be validated if is in fact a Guid
                Amount = expectedAmount,
                ValidFrom = startPeriodDateTime,
                ValidTo = expectedEndPeriodDateTime,
                LicensePlate = licensePlate,
                WasUsed = false
            };

            //ACT
            var parkingTicket = buyTicketLogicHandler.BuildParkingTicketMemoryEntity(buyTicketRequest);

            //ASSERT
            parkingTicket.Should().BeEquivalentTo(expectedParkingTicketEntity, EquivalencyAssertionOptions);
        }


        private EquivalencyAssertionOptions<ParkingTicket> EquivalencyAssertionOptions(EquivalencyAssertionOptions<ParkingTicket> options) => options.Excluding(t => t.TicketId)
            .Using<Guid>(ctx =>
            {
                ctx.Subject.Should().NotBe(Guid.Empty, "TicketId should be a valid, non-empty GUID");
            })
            .When(info => info.Path.EndsWith(nameof(ParkingTicket.TicketId)));
    }
}
