using Entity.DBModels;

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

        public class TransferReqeuest
        {
            public string FromIBAN { get; set; }
            public string ToIBAN { get; set; }
            public decimal Amount { get; set; }
            public string Remark { get; set; }
        }

        public class TransferResponse
        {
            public TransferResponse() { }

            public TransferResponse(Transaction transaction, Account transferAccount, Account receivedAccount)
            {
                TransactionId = transaction.Id;
                TransferIBAN = transferAccount.IBAN;
                ReceiveIBAN = receivedAccount.IBAN;
                ReceiveCustomerFullName = (receivedAccount.Customer != null) ? $"{receivedAccount.Customer.FirstName} {receivedAccount.Customer.LastName}" : "-";
                Amount = transaction.Amount;
                Fee = transaction.Fee;
            }

            public int TransactionId { get; set; }
            public string TransferIBAN { get; set; }
            public string ReceiveIBAN { get; set; }
            public string ReceiveCustomerFullName { get; set; }
            public decimal Amount { get; set; }
            public decimal Fee { get; set; }
        }

    }
}
