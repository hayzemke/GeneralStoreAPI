using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TransactionDetail
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public DateTime? DateOfTransaction { get; set; } = DateTime.Now;
    public int? ProductId { get; set; }
    public int? CustomerId { get; set; }

    public string CustomerName { get; set; }
    public string ProductName { get; set; }
}