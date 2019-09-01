using Entity.DBModels;

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
