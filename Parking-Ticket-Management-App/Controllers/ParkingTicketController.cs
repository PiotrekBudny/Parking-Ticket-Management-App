using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Parking_Ticket_Management_App.Controllers.Models;
using Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket;
using Parking_Ticket_Management_App.Controllers.Models.IsAlive;
using Parking_Ticket_Management_App.Controllers.Models.UseTicket;
using Parking_Ticket_Management_App.Controllers.Models.ValidateTicket;

namespace Parking_Ticket_Management_App.Controllers
{
    [ApiController]
    [Route("parking-ticket")]
    public class ParkingTicketController : ControllerBase
    {
        private readonly ILogger<ParkingTicketController> _logger;
        private readonly IParkingTicketHandler _parkingTicketHandler;
        private IValidator _buyTicketRequestValidator;
        private IValidator _validateTicketRequestValidator;
        private IValidator _useTicketRequestValidator;

        public ParkingTicketController(ILogger<ParkingTicketController> logger,
            IParkingTicketHandler parkingTicketHandler,
            IValidator<BuyTicketRequest> buyTicketRequestValidator,
            IValidator<ValidateTicketRequest> validateTicketRequestValidator,
            IValidator<UseTicketRequest> useTicketRequestValidator)
        {
            _logger = logger;
            _parkingTicketHandler = parkingTicketHandler;
            _buyTicketRequestValidator = buyTicketRequestValidator;
            _validateTicketRequestValidator = validateTicketRequestValidator;
            _useTicketRequestValidator = useTicketRequestValidator;
        }


        [HttpGet("is-alive")]
        public IActionResult IsAlive()
        {
            _logger.LogInformation("Checking if the Parking Ticket Management App is alive.");

            return Ok(new IsAliveResponse { IsAlive = true });
        }

        [HttpPost("buy")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(BuyTicketResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicketRequest buyTicketRequest)
        {
            _logger.LogInformation("BuyTicket endpoint called.");

            if (!ModelState.IsValid) return BadRequest(new ErrorResponse { Message = "Model binding failed." });

            var validationResult = await _buyTicketRequestValidator.ValidateAsync(new ValidationContext<BuyTicketRequest>(buyTicketRequest));

            if (!validationResult.IsValid)
            {
                var validationMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

                _logger.LogWarning($"BuyTicket request validation failed: {validationMessage}");

                return BadRequest(new ErrorResponse { Message = $"Validation failed. {validationMessage}" });
            }

            try
            {
                var response = _parkingTicketHandler.HandleBuyTicketRequest(buyTicketRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the BuyTicket request.");
                return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("validate")]
        [ProducesResponseType(typeof(ValidateTicketResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> ValidateTicket([FromQuery] ValidateTicketRequest validateTicketRequest)
        {
            _logger.LogInformation("ValidateTicket endpoint called.");

            if (!ModelState.IsValid) return BadRequest(new ErrorResponse { Message = "Model binding failed." });

            var validationResult = await _validateTicketRequestValidator.ValidateAsync(new ValidationContext<ValidateTicketRequest>(validateTicketRequest));

            if (!validationResult.IsValid)
            {
                var validationMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

                _logger.LogWarning($"ValidateTicket request validation failed: {validationMessage}");

                return BadRequest(new ErrorResponse { Message = $"Validation failed. {validationMessage}" });
            }

            try
            {
                var response = _parkingTicketHandler.HandleValidateTicketRequest(validateTicketRequest);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the ValidateTicket request.");

                return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
            }
        }


        [HttpPatch("{TicketId:guid}/use")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UseTicket([FromRoute] UseTicketRequest useTicketRequest)
        {
            _logger.LogInformation("UseTicket endpoint called.");
            if (!ModelState.IsValid) return BadRequest(new ErrorResponse { Message = "Model binding failed." });

            var validationResult = await _useTicketRequestValidator.ValidateAsync(new ValidationContext<UseTicketRequest>(useTicketRequest));

            if (!validationResult.IsValid)
            {
                var validationMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

                _logger.LogWarning($"UseTicket request validation failed: {validationMessage}");

                return BadRequest(new ErrorResponse { Message = $"Validation failed. {validationMessage}" });
            }

            try
            {
                _parkingTicketHandler.HandleUseTicketRequest(useTicketRequest);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogDebug(ex, "UseTicket: Ticket with id {TicketId} not found.", useTicketRequest.TicketId);
                return NotFound(new ErrorResponse { Message = $"Ticket not found: {ex.Message}" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogDebug(ex, "An error occurred while processing the UseTicket request.");
                return BadRequest(new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the UseTicket request.");
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: { ex.Message }"});
            }
        }
    }
}
