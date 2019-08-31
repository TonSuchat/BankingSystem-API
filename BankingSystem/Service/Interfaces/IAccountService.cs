using Entity.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<string> CreateAccount(Customer customer, decimal initialMoney = 0);
    }
}
