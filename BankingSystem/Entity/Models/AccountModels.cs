using Entity.DBModels;

namespace Entity.Models
{
    public class AccountModels
    {

        public class CreateAccountResponse
        {
            public CreateAccountResponse() { }

            public CreateAccountResponse(Account account, Customer customer)
            {
                IBAN = account.IBAN;
                FirstName = customer.FirstName;
                LastName = customer.LastName;
                TotalAmount = account.TotalAmount;
            }

            public string IBAN { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public decimal TotalAmount { get; set; }
        }
        
    }
}
