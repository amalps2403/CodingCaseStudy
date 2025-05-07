using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAC.IntegrationSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;

        public SalesOrderController(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var salesOrders = await _salesOrderService.GetAllAsync();
            return Ok(salesOrders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var salesOrder = await _salesOrderService.GetByIdAsync(id);
            if (salesOrder == null)
                return NotFound();
            return Ok(salesOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SalesOrder salesOrder)
        {
            var createdSalesOrder = await _salesOrderService.CreateAsync(salesOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdSalesOrder.Id }, createdSalesOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SalesOrder salesOrder)
        {
            if (id != salesOrder.Id)
                return BadRequest();

            var success = await _salesOrderService.UpdateAsync(salesOrder);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _salesOrderService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
