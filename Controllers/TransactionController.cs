using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly GeneralStoreDBContext _db;

    public TransactionController(ILogger<TransactionController> logger, GeneralStoreDBContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromForm] TransactionEdit newTransaction)
    {
        Transaction transaction = new Transaction()
        {
            ProductId = newTransaction.ProductId,
            CustomerId = newTransaction.CustomerId,
            Quantity = newTransaction.Quantity,
        };
        
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _db.Transactions.ToListAsync();
        return Ok(transactions);
    }

    //todo UPDATE
    [HttpGet, Route("{id}")]
    public async Task<IActionResult> GetTransactionByID(int id)
    {
        if(id<1)
            return BadRequest();
        
        var transaction = await _db.Transactions.Include(t=> t.Customer).Include(t=> t.Product).FirstOrDefaultAsync(t=> t.Id == id);

        if (transaction is null)
        return NotFound();

        var transactionDetail = new TransactionDetail
        {
            Id=transaction.Id,
            Quantity=transaction.Quantity,
            DateOfTransaction=transaction.DateOfTransaction,
            ProductId=transaction.ProductId,
            CustomerId=transaction.CustomerId,
            CustomerName=transaction.Customer.Name,
            ProductName=transaction.Product.Name,
        };

        return Ok(transactionDetail);
    }

    [HttpPut, Route("{id}")]
    public async Task<IActionResult> UpdateTransaction([FromForm]TransactionEdit model, int id)
    {
        if(id<1)
            return BadRequest();

        var transaction=await _db.Transactions.FindAsync(id);

        if(transaction is null)
            return NotFound();

        transaction.Quantity=model.Quantity;
        transaction.ProductId=model.ProductId;
        transaction.CustomerId=model.CustomerId;

        await _db.SaveChangesAsync();
        return Ok();
    }

    //todo DELETE
    [HttpDelete, Route("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        if(id<1)
            return BadRequest();

        var transaction = await _db.Transactions.FindAsync(id);

        if(transaction is null)
            return NotFound();

        _db.Transactions.Remove(transaction);
        await _db.SaveChangesAsync();
        return Ok();
    }
}