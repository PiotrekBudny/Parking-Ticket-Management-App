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
                    Amount = 150.00m,
                    WasUsed = false
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
                    Amount = 150.00m,
                    WasUsed = false
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
                    Amount = 150.00m,
                    WasUsed = false
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 5, 15, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m,
                    WasUsed = false
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 10, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 5, 30, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m,
                    WasUsed = false
                }
            };
            
            var currentTime = new DateTime(2024, 5, 15, 10, 0, 0, DateTimeKind.Utc);

            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);
            
            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfParkingTicketsWithMultipleEntries);
            
            //ASSERT
            result.Should().BeEquivalentTo(listOfParkingTicketsWithMultipleEntries.OrderBy(x => x.ValidTo).First());
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

        [Test] 
        public void WillReturnUsedTicketOverUnusedWhenBothAreValid()
        {
            //ARRANGE
            var validatedLicensePlate = "AB12345";
            var listOfParkingTicketsWithUsedAndUnusedEntries = new List<ParkingTicket>
            {
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 150.00m,
                    WasUsed = false
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 6, 15, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m,
                    WasUsed = true
                }
            };
            
            var currentTime = new DateTime(2024, 6, 10, 10, 0, 0, DateTimeKind.Utc);
            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);
            
            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfParkingTicketsWithUsedAndUnusedEntries);
            
            //ASSERT
            result.Should().BeEquivalentTo(listOfParkingTicketsWithUsedAndUnusedEntries.Where(t => t.WasUsed).OrderBy(t => t.ValidTo).First());
        }

        [Test]
        public void WillReturnNullWhenAllTicketsAreExpired()
        {
            //ARRANGE
            var validatedLicensePlate = "AB12345";
            var listOfExpiredParkingTickets = new List<ParkingTicket>
            {
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 4, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 4, 30, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 150.00m,
                    WasUsed = false
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 3, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 3, 31, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m,
                    WasUsed = true
                }
            };
            
            var currentTime = new DateTime(2024, 6, 10, 10, 0, 0, DateTimeKind.Utc);
            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);
            
            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfExpiredParkingTickets);
            
            //ASSERT
            result.Should().BeNull();
        }

        [Test]
        public void WillReturnNullWhenAllTicketsAreNotYetValid()
        {
            //ARRANGE
            var validatedLicensePlate = "AB12345";
            var listOfFutureParkingTickets = new List<ParkingTicket>
            {
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 7, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 7, 31, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 150.00m,
                    WasUsed = false
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 8, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 8, 31, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m,
                    WasUsed = true
                }
            };
            
            var currentTime = new DateTime(2024, 6, 10, 10, 0, 0, DateTimeKind.Utc);
            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);
            
            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfFutureParkingTickets);
            
            //ASSERT
            result.Should().BeNull();
        }

        [Test]
        public void WillReturnUnusedTicketWhenUsedTicketIsNoLongerValid()
        {
            //ARRANGE
            var validatedLicensePlate = "AB12345";
            var listOfParkingTicketsWithUsedAndUnusedEntries = new List<ParkingTicket>
            {
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 6, 30, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 150.00m,
                    WasUsed = false
                },
                new() {
                    TicketId = Guid.NewGuid(),
                    LicensePlate = validatedLicensePlate,
                    ValidFrom = new DateTime(2024, 5, 1, 22, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2024, 6, 15, 22, 0, 0, DateTimeKind.Utc),
                    Amount = 75.00m,
                    WasUsed = true
                }
            };

            var currentTime = new DateTime(2024, 6, 16, 10, 0, 0, DateTimeKind.Utc);
            _mockSystemDateTimeProvider.Setup(s => s.UtcNow).Returns(currentTime);
            var validateTicketLogicHandler = new ValidateTicketLogicHandler(_mockSystemDateTimeProvider.Object);

            //ACT
            var result = validateTicketLogicHandler.ValidateTicket(listOfParkingTicketsWithUsedAndUnusedEntries);

            //ASSERT
            result.Should().BeEquivalentTo(listOfParkingTicketsWithUsedAndUnusedEntries.Where(t => !t.WasUsed).OrderBy(t => t.ValidTo).First());
        }
    }
}
