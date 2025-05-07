using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAC.IntegrationSolution.API.Controllers
{
    [Route("api/wms/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceiveOrder([FromBody] object order)
        {
            // do further operations based on the business logic - like place oder in the db and send a response
            Console.WriteLine("Received Order: " + order.ToString());
            return Ok(new { message = "Order received successfully" });
        }
    }

}
