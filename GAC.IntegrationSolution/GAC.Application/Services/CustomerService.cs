using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using GAC.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GAC.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        /// <returns>A list of all customers.</returns>

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>The customer if found, otherwise null.</returns>
        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }


        /// <summary>
        /// Creates a new customer in the database.
        /// </summary>
        /// <param name="customer">The customer entity to create.</param>
        /// <returns>The created customer entity.</returns>
        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        /// <summary>
        /// Updates an existing customer in the database.
        /// </summary>
        /// <param name="customer">The customer entity with updated information.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes a customer from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
