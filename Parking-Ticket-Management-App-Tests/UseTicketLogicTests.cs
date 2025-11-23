using FluentAssertions;
using Moq;
using Parking_Ticket_Management_App.Logic;
using Parking_Ticket_Management_App.Memory.Models;
using Parking_Ticket_Management_App.Utils;

namespace Parking_Ticket_Management_App_Tests
{
    public class UseTicketLogicTests
    {
        private Mock<ISystemDateTimeProvider> _systemDateTimeProviderMock;

        [SetUp]
        public void Setup()
        {
            _systemDateTimeProviderMock = new Mock<ISystemDateTimeProvider>();
        }

        [Test]
        public void WillUpdateTicketAsUsedWhenTicketIsCurrentlyValid()
        {
            //ARRANGE
            var ticketId = Guid.NewGuid();
            var initialValidFrom = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc);
            var initialValidTo = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc);
            var parkingTicketBeforeUse = new ParkingTicket
            {
                TicketId = ticketId,
                LicensePlate = "AB12345",
                ValidFrom = initialValidFrom,
                ValidTo = initialValidTo,
                Amount = 150.00m,
                WasUsed = false
            };

            var expectedParkingTicketAfterUse = TransformSetupTicketToExpected(parkingTicketBeforeUse);
            var currentTime = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
            _systemDateTimeProviderMock.Setup(s => s.UtcNow).Returns(currentTime);

            var useTicketLogicHandler = new UseTicketLogicHandler(_systemDateTimeProviderMock.Object);
            //ACT
            var updatedTicket = useTicketLogicHandler.MarkTicketEntityAsUsed(parkingTicketBeforeUse);
            //ASSERT
            updatedTicket.Should().BeEquivalentTo(expectedParkingTicketAfterUse);
        }

        [Test]
        public void ExceptionIsThrownWhenUsingAlreadyUsedTicket()
        {
            //ARRANGE
            var parkingTicket = new ParkingTicket
            {
                TicketId = Guid.NewGuid(),
                LicensePlate = "AB12345",
                ValidFrom = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                ValidTo = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc),
                Amount = 150.00m,
                WasUsed = true
            };

            var currentTime = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
            _systemDateTimeProviderMock.Setup(s => s.UtcNow).Returns(currentTime);

            var useTicketLogicHandler = new UseTicketLogicHandler(_systemDateTimeProviderMock.Object);
            //ACT
            Action act = () => useTicketLogicHandler.MarkTicketEntityAsUsed(parkingTicket);
            //ASSERT
            act.Should().Throw<InvalidOperationException>().WithMessage($"Ticket with ID {parkingTicket.TicketId} has already been used.");
        }

        [Test]
        public void ExceptionIsThrownWhenUsingExpiredTicket()
        {
            //ARRANGE
            var parkingTicket = new ParkingTicket
            {
                TicketId = Guid.NewGuid(),
                LicensePlate = "AB12345",
                ValidFrom = new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                ValidTo = new DateTime(2024, 5, 31, 22, 0, 0, DateTimeKind.Utc),
                Amount = 150.00m,
                WasUsed = false
            };
            var currentTime = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
            _systemDateTimeProviderMock.Setup(s => s.UtcNow).Returns(currentTime);

            var useTicketLogicHandler = new UseTicketLogicHandler(_systemDateTimeProviderMock.Object);
            //ACT
            Action act = () => useTicketLogicHandler.MarkTicketEntityAsUsed(parkingTicket);
            //ASSERT
            act.Should().Throw<InvalidOperationException>().WithMessage($"Ticket with ID {parkingTicket.TicketId} is not valid at the current time.");
        }

        private ParkingTicket TransformSetupTicketToExpected(ParkingTicket parkingTicket)
        {
            var newParkingTicket = new ParkingTicket
            {
                TicketId = parkingTicket.TicketId,
                LicensePlate = parkingTicket.LicensePlate,
                ValidFrom = parkingTicket.ValidFrom,
                ValidTo = parkingTicket.ValidTo,
                Amount = parkingTicket.Amount,
                WasUsed = true
            };

            return newParkingTicket;
        }
    }
}
