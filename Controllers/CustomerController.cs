using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly GeneralStoreDBContext _db;

    public CustomerController(ILogger<CustomerController> logger, GeneralStoreDBContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromForm] CustomerEdit newCustomer)
    {
        Customer customer = new Customer()
        {
            Name = newCustomer.Name,
            Email = newCustomer.Email,
        };
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _db.Customers.ToListAsync();
        return Ok(customers);
    }

    //todo UPDATE

    [HttpGet, Route("{id}")]
    public async Task<IActionResult> GetCustomerByID(int id)
    {
        if (id < 1)
            return BadRequest();

        var customer = await _db.Customers.FindAsync(id);

        if (customer is null)
            return NotFound();

        else
            return Ok(customer);
    }

    [HttpPut, Route("{id}")]
    public async Task<IActionResult> UpdateCustomer([FromForm] CustomerEdit model, int id)
    {
        if (id < 1)
            return BadRequest();

        var customer = await _db.Customers.FindAsync(id);

        if (customer is null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(model.Name))
            customer.Name = model.Name;

        if (!string.IsNullOrWhiteSpace(model.Email))
            customer.Email = model.Email;

        await _db.SaveChangesAsync();
        return Ok();
    }

    //todo DELETE
    [HttpDelete, Route("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        if(id<1)
            return BadRequest();

        var customer = await _db.Customers.FindAsync(id);

        if(customer is null)
        return NotFound();

        else
        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();
        return Ok();
    }
}