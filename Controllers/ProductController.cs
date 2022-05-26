using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;

    private readonly GeneralStoreDBContext _db;

    public ProductController(ILogger<ProductController> logger, GeneralStoreDBContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductEdit newProduct)
    {
        Product product = new Product()
        {
            Name = newProduct.Name,
            Price = newProduct.Price,
            QuantityInStock = newProduct.Quantity,
        };
        
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _db.Products.ToListAsync();
        return Ok(products);
    }

    //todo UPDATE
    [HttpGet, Route("{id}")]
    public async Task<IActionResult> GetProductByID(int id)
    {
        if (id < 1)
            return BadRequest();

        var product = await _db.Products.FindAsync(id);
        return (product is null) ? NotFound() : Ok(product);
    }

    [HttpPut, Route("{id}")]
    public async Task<IActionResult> UpdateProduct([FromForm] ProductEdit model, int id)
    {
        if (id < 1)
            return BadRequest();

        var product = await _db.Products.FindAsync(id);

        if (product is null)
            return NotFound();


        if (!string.IsNullOrWhiteSpace(model.Name))
            product.Name = model.Name;

        product.QuantityInStock = model.Quantity;

        product.Price = model.Price;

        await _db.SaveChangesAsync();
        return Ok();
    }

    //todo DELETE

    [HttpDelete, Route("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        else
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}