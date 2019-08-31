using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class Account
    {
        [Key]
        [MaxLength(100)]
        public string IBAN { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public Customer Customer { get; set; }
        public ICollection<Transaction> TransferTransactions { get; set; }
        public ICollection<Transaction> ReceivedTransactions { get; set; }
    }
}
