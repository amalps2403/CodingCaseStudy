using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using GAC.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GAC.Application.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly AppDbContext _context;

        public SalesOrderService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all sales orders, including their items and associated products.
        /// </summary>
        /// <returns>A list of all sales orders with their details.</returns>
        public async Task<IEnumerable<SalesOrder>> GetAllAsync()
        {
            return await _context.SalesOrders
                                 .Include(so => so.SalesOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific sales order by its unique identifier, including its items and associated products.
        /// </summary>
        /// <param name="id">The unique identifier of the sales order.</param>
        /// <returns>The sales order if found, otherwise null.</returns>
        public async Task<SalesOrder?> GetByIdAsync(Guid id)
        {
            return await _context.SalesOrders
                                 .Include(so => so.SalesOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .FirstOrDefaultAsync(so => so.Id == id);
        }

        /// <summary>
        /// Creates a new sales order and saves it to the database.
        /// </summary>
        /// <param name="so">The sales order to create.</param>
        /// <returns>The created sales order.</returns>
        public async Task<SalesOrder> CreateAsync(SalesOrder so)
        {
            _context.SalesOrders.Add(so);
            await _context.SaveChangesAsync();
            return so;
        }

        /// <summary>
        /// Updates an existing sales order in the database.
        /// </summary>
        /// <param name="so">The sales order to update.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateAsync(SalesOrder so)
        {
            _context.SalesOrders.Update(so);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes a sales order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sales order to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var so = await _context.SalesOrders.FindAsync(id);
            if (so == null) return false;

            _context.SalesOrders.Remove(so);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
