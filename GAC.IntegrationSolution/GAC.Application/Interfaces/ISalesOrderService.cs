using GAC.Application.DTOs;
using GAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Application.Interfaces
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrder>> GetAllAsync();
        Task<SalesOrder?> GetByIdAsync(Guid id);
        Task<SalesOrder> CreateAsync(SalesOrder so);
        Task<bool> UpdateAsync(SalesOrder so);
        Task<bool> DeleteAsync(Guid id);
    }
}
