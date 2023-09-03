using Microsoft.AspNetCore.Mvc;
using CustomersApi.Dtos;
using CustomersApi.Repositories;
using CustomersApi.CasosDeUso;

namespace CustomersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly CustomerDatabaseContext _customerDatabaseContext;
        private readonly IUpdateCustomerUseCase _updateCustomerUseCase;

        public CustomerController(CustomerDatabaseContext customerDatabaseContext,
            IUpdateCustomerUseCase updateCustomerUseCase)
        {
            _customerDatabaseContext = customerDatabaseContext;
            _updateCustomerUseCase = updateCustomerUseCase;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerDto>))]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var result = _customerDatabaseContext.Customers
                    .Select(c => c.ToDto()).ToList();

                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomer(long id)
        {
            try
            {
                CustomerEntity result = await _customerDatabaseContext.Get(id);
                if (result == null)
                    return new NotFoundResult();

                return new OkObjectResult(result.ToDto());
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            try
            {
                var result = await _customerDatabaseContext.Delete(id);
                
                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerDto))]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto customer)
        {
            try
            {
                CustomerEntity result = await _customerDatabaseContext.Add(customer);
            
                return new CreatedResult($"https://localhost:7246/api/customer/{result.Id}", null);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(CustomerDto customer)
        {
            try
            {
                CustomerDto? result = await _updateCustomerUseCase.Execute(customer);

                if (result == null)
                    return new NotFoundResult();
            
                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}