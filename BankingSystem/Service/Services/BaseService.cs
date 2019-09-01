using Entity.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Services
{
    public class BaseService
    {

        public readonly BankingSystemContext _context;

        public BaseService(BankingSystemContext context)
        {
            _context = context;
        }

    }
}
