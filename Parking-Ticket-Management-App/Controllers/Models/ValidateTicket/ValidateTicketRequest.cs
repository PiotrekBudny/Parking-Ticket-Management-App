using FluentValidation;

namespace Parking_Ticket_Management_App.Controllers.Models.ValidateTicket
{
    public class ValidateTicketRequest
    {
        public string LicensePlate { get; set; } = null!;
    }

    public class ValidateTicketRequestValidator : AbstractValidator<ValidateTicketRequest>
    {
        public ValidateTicketRequestValidator()
        {
            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("License plate is required.")
                .Length(3, 10).WithMessage("License plate must be between 3 and 10 characters.");
        }
    }
}
