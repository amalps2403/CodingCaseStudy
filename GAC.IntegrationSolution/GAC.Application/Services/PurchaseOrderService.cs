using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using GAC.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GAC.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly AppDbContext _context;

        public PurchaseOrderService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all purchase orders, including their items and associated products.
        /// </summary>
        /// <returns>A list of all purchase orders with their details.</returns>
        public async Task<IEnumerable<PurchaseOrder>> GetAllAsync()
        {
            return await _context.PurchaseOrders
                                 .Include(po => po.PurchaseOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific purchase order by its unique identifier, including its items and associated products.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase order.</param>
        /// <returns>The purchase order if found, otherwise null.</returns>
        public async Task<PurchaseOrder?> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseOrders
                                 .Include(po => po.PurchaseOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .FirstOrDefaultAsync(po => po.Id == id);
        }

        /// <summary>
        /// Creates a new purchase order and saves it to the database.
        /// </summary>
        /// <param name="po">The purchase order to create.</param>
        /// <returns>The created purchase order.</returns>
        public async Task<PurchaseOrder> CreateAsync(PurchaseOrder po)
        {
            _context.PurchaseOrders.Add(po);
            await _context.SaveChangesAsync();
            return po;
        }

        /// <summary>
        /// Updates an existing purchase order in the database.
        /// </summary>
        /// <param name="po">The purchase order with updated details.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateAsync(PurchaseOrder po)
        {
            _context.PurchaseOrders.Update(po);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes a purchase order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase order to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null) return false;

            _context.PurchaseOrders.Remove(po);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
