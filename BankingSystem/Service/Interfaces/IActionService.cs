using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Entity.Models.ActionModels;

namespace Service.Interfaces
{
    public interface IActionService
    {
        Task<DepositResponse> Deposit(string iban, decimal depositMoney, string remark = null);
    }
}
