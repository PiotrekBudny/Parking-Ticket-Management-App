using Microsoft.AspNetCore.Mvc;
using Parking_Ticket_Management_App.Controllers.Models.IsAlive;

namespace Parking_Ticket_Management_App.Controllers
{
    [ApiController]
    [Route("parking-ticket")]
    public class ParkingTicketController : ControllerBase
    {
        private readonly ILogger<ParkingTicketController> _logger;

        public ParkingTicketController(ILogger<ParkingTicketController> logger)
        {
            _logger = logger;
        }


        [HttpGet("is-alive")]
        public IActionResult IsAlive()
        {
            _logger.LogInformation("Checking if the Parking Ticket Management App is alive.");

            return Ok(new IsAliveResponse { IsAlive = true });
        }
    }
}
