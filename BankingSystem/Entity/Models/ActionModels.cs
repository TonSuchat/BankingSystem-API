using Entity.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Models
{
    public class ActionModels
    {
        public class DepositRequest
        {
            public string IBAN { get; set; }
            public decimal DepositMoney { get; set; }
            public string Remark { get; set; }
        }

        public class DepositResponse
        {
            public DepositResponse() { }

            public DepositResponse(Transaction transaction, Account account)
            {
                TransactionId = transaction.Id;
                IBAN = account.IBAN;
                Deposit = transaction.Amount;
                Fee = transaction.Fee;
                TotalAmount = account.TotalAmount;
            }

            public int TransactionId { get; set; }
            public string IBAN { get; set; }
            public decimal Deposit { get; set; }
            public decimal Fee { get; set; }
            public decimal TotalAmount { get; set; }
        }
    }
}
