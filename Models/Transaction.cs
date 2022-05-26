using System;
using System.Collections.Generic;

    public partial class Transaction
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime? DateOfTransaction { get; set; } = DateTime.Now;
        public int? ProductId { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
