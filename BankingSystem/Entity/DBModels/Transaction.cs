using Entity.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.DBModels
{
    public class Transaction
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public string TransferIBAN { get; set; }
        public string ReceiveIBAN { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Remark { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public Account TransferAccount { get; set; }
        public Account ReceiveAccount { get; set; }
    }
}
