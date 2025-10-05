using FluentAssertions;
using Moq;
using Parking_Ticket_Management_App.Logic;
using Parking_Ticket_Management_App.Memory.Models;
using Parking_Ticket_Management_App.Utils;

namespace Parking_Ticket_Management_App_Tests
{
    public class ValidateTicketLogicTests
    {
        private Mock<ISystemDateTimeProvider> _mockSystemDateTimeProvider;

        [SetUp]
        public void Setup()
        {
            _mockSystemDateTimeProvider = new Mock<ISystemDateTimeProvider>();
        }

        [Test]
        public void WillReturnSingleValidTicketWhenOnlyOneTicketIsOnList()
        {
            //ARRANGE
            var validatedLicensePlate = "AB12345";
            var listOfParkingTicketsWithOnlyOneEntry = new List<ParkingTicket>
            {
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 31, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 150.00m
                }
            };

            var currentTime = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);
            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);

            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfParkingTicketsWithOnlyOneEntry);
            
            //ASSERT
            result.Should().BeEquivalentTo(listOfParkingTicketsWithOnlyOneEntry.First());
        }

        [Test]
        public void WillReturnNullWhenNoValidTicketIsFoundInTheList()
        {
            //ARRANGE
            var validatedLicensePlate = "AB12345";
            var listOfParkingTicketsWithOnlyOneEntry = new List<ParkingTicket>
            {
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 5, 31, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 150.00m
                }
            };
            
            var currentTime = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);
            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);

            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);
            
            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfParkingTicketsWithOnlyOneEntry);
            
            //ASSERT
            result.Should().BeNull();
        }

        [Test]
        public void WillReturnTheFirstValidTicketWhenMultipleTicketsAreOnTheList()
        {
            //ARRANGE
            var validatedLicensePlate = "AB12345";
            var listOfParkingTicketsWithMultipleEntries = new List<ParkingTicket>
            {
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 5, 31, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 150.00m
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 5, 15, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 10, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 5, 30, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m
                }
            };
            
            var currentTime = new DateTime(2024, 5, 15, 10, 0, 0, DateTimeKind.Utc);

            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);
            
            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfParkingTicketsWithMultipleEntries);
            
            //ASSERT
            result.Should().BeEquivalentTo(listOfParkingTicketsWithMultipleEntries.First());
        }

        [Test]
        public void WillReturnNullWhenNoTicketsAreOnTheList()
        {
            //ARRANGE
            var listOfParkingTicketsWithNoEntries = new List<ParkingTicket>();
            var currentTime = new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);
            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);
            
            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfParkingTicketsWithNoEntries);
            
            //ASSERT
            result.Should().BeNull();
        }
    }
}
