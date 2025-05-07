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
    public class SalesOrderService : ISalesOrderService
    {
        private readonly AppDbContext _context;

        public SalesOrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesOrder>> GetAllAsync()
        {
            return await _context.SalesOrders
                                 .Include(so => so.SalesOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .ToListAsync();
        }

        public async Task<SalesOrder?> GetByIdAsync(Guid id)
        {
            return await _context.SalesOrders
                                 .Include(so => so.SalesOrderItems)
                                 .ThenInclude(item => item.Product)
                                 .FirstOrDefaultAsync(so => so.Id == id);
        }

        public async Task<SalesOrder> CreateAsync(SalesOrder so)
        {
            _context.SalesOrders.Add(so);
            await _context.SaveChangesAsync();
            return so;
        }

        public async Task<bool> UpdateAsync(SalesOrder so)
        {
            _context.SalesOrders.Update(so);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var so = await _context.SalesOrders.FindAsync(id);
            if (so == null) return false;

            _context.SalesOrders.Remove(so);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
