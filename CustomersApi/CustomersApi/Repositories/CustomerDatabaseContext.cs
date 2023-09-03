using CustomersApi.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomersApi.Repositories
{
    public class CustomerDatabaseContext : DbContext
    {
        public CustomerDatabaseContext(DbContextOptions<CustomerDatabaseContext> options)
            : base(options)
        {

        }

        public DbSet<CustomerEntity> Customers { get; set; }

        public async Task<CustomerEntity?> Get(long id)
        {
            var customer = await Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer == null)
                throw new ArgumentNullException("No se ha encontrado el cliente con el id proporcionado");

            return customer;
        }

        public async Task<bool> Delete(long id)
        {
            CustomerEntity entity = await Get(id);
            if (entity == null)
                throw new ArgumentNullException("No se ha encontrado el cliente para eliminar");

            Customers.Remove(entity);
            var deleted = await SaveChangesAsync();
            
            return deleted > 0;
        }

        public async Task<CustomerEntity> Add(CreateCustomerDto customerDto)
        {
            CustomerEntity entity = new CustomerEntity()
            {
                Id = null,
                Address = customerDto.Address,
                Email = customerDto.Email,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Phone = customerDto.Phone,
            };

            EntityEntry<CustomerEntity> response = await Customers.AddAsync(entity);
            await SaveChangesAsync();
            return await Get(response.Entity.Id ?? throw new Exception("no se ha podido guardar el cliente"));

        }

        public async Task<bool> Actualizar(CustomerEntity customerEntity)
        {
            Customers.Update(customerEntity);
            var updated = await SaveChangesAsync();

            return updated > 0;
        }

    }


    public class CustomerEntity
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }


        public CustomerDto ToDto()
        {
            return new CustomerDto()
            {
                Address = Address,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Id = Id ?? throw new Exception("el id no puede ser null")
            };
        }
    }
}