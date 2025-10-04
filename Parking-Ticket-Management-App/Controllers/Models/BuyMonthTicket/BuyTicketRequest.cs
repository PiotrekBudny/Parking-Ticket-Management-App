using FluentValidation;

namespace Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket
{
    public class BuyTicketRequest
    {
        public string LicensePlate { get; set; } = null!;
    }

    public class BuyTicketRequestValidator : AbstractValidator<BuyTicketRequest>
    {
        public BuyTicketRequestValidator()
        {
            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("License plate is required.")
                .Length(3, 10).WithMessage("License plate must be between 3 and 10 characters.");
        }
    }
}
