using Entity.DBModels;
using System.Threading.Tasks;
using static Entity.Models.AccountModels;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<CreateAccountResponse> CreateAccount(Customer customer, decimal initialMoney = 0);
    }
}
