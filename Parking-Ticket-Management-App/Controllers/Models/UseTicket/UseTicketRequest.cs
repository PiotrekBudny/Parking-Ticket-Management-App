using FluentValidation;
using Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket;

namespace Parking_Ticket_Management_App.Controllers.Models.UseTicket
{
    public class UseTicketRequest
    {
        public Guid TicketId { get; set; }
    }

    public class UseTicketRequestValidator : AbstractValidator<UseTicketRequest>
    {
        public UseTicketRequestValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty().WithMessage("TicketId is required.")
                .Must(id => id != Guid.Empty).WithMessage("TicketId must be a valid GUID.");  
        }
    }
}
