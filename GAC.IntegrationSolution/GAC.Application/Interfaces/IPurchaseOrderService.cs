using GAC.Application.DTOs;
using GAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Application.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrder>> GetAllAsync();
        Task<PurchaseOrder?> GetByIdAsync(Guid id);
        Task<PurchaseOrder> CreateAsync(PurchaseOrder po);
        Task<bool> UpdateAsync(PurchaseOrder po);
        Task<bool> DeleteAsync(Guid id);
    }
}
