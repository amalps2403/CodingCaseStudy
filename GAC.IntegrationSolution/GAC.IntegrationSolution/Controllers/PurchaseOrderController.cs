using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAC.IntegrationSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchaseOrders = await _purchaseOrderService.GetAllAsync();
            return Ok(purchaseOrders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var purchaseOrder = await _purchaseOrderService.GetByIdAsync(id);
            if (purchaseOrder == null)
                return NotFound();
            return Ok(purchaseOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseOrder purchaseOrder)
        {
            var createdPurchaseOrder = await _purchaseOrderService.CreateAsync(purchaseOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdPurchaseOrder.Id }, createdPurchaseOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.Id)
                return BadRequest();

            var success = await _purchaseOrderService.UpdateAsync(purchaseOrder);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _purchaseOrderService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
