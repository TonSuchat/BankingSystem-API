using Entity.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public TransactionType Type { get; set; }
        public string TransferIBAN { get; set; }
        public string ReceiveIBAN { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        [MaxLength(2500)]
        public string Remark { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public Account TransferAccount { get; set; }
        public Account ReceiveAccount { get; set; }
    }
}
