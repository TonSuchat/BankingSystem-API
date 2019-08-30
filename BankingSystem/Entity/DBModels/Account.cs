using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class Account
    {
        [Key]
        public string IBAN { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public Customer Customer { get; set; }
        public ICollection<Transaction> TransferTransactions { get; set; }
        public ICollection<Transaction> ReceivedTransactions { get; set; }
    }
}
