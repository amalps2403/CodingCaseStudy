using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using GAC.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly AppDbContext _context;

        public PurchaseOrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllAsync()
        {
            return await _context.PurchaseOrders
                                 .Include(po => po.PurchaseOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .ToListAsync();
        }

        public async Task<PurchaseOrder?> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseOrders
                                 .Include(po => po.PurchaseOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .FirstOrDefaultAsync(po => po.Id == id);
        }

        public async Task<PurchaseOrder> CreateAsync(PurchaseOrder po)
        {
            _context.PurchaseOrders.Add(po);
            await _context.SaveChangesAsync();
            return po;
        }

        public async Task<bool> UpdateAsync(PurchaseOrder po)
        {
            _context.PurchaseOrders.Update(po);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null) return false;

            _context.PurchaseOrders.Remove(po);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
