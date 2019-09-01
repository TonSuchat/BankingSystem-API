using Entity.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Entity.Models.AccountModels;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<CreateAccountResponse> CreateAccount(Customer customer, decimal initialMoney = 0);
        Task<Account> GetAccount(string iban);
        Task<IList<Account>> GetAccounts();
    }
}
