﻿using Entity.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.DBModels
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public void Deposit(Account account, decimal amount, decimal fee, string remark = null)
        {
            Type = TransactionType.DEPOSIT;
            ReceiveIBAN = account.IBAN;
            Amount = amount;
            Fee = fee;
            Remark = remark;
        }

        public void Transfer(string transferIBAN, string receiveIBAN, decimal amount, string remark = null)
        {
            Type = TransactionType.TRANSFER;
            TransferIBAN = transferIBAN;
            ReceiveIBAN = receiveIBAN;
            Amount = amount;
            Remark = remark;
        }

    }
}
