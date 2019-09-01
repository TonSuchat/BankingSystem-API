using System.Threading.Tasks;
using static Entity.Models.ActionModels;

namespace Service.Interfaces
{
    public interface IActionService
    {
        Task<DepositResponse> Deposit(string iban, decimal depositMoney, string remark = null);
        Task<TransferResponse> Transfer(string fromIBAN, string toIBAN, decimal amount, string remark = null);
    }
}
