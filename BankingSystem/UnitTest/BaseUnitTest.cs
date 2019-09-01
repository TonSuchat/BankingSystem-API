using Entity.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service;

namespace UnitTest
{
    public class BaseUnitTest
    {

        public DbContextOptions<BankingSystemContext> GetInMemoryOptions(string dbName = "BankingSystemDBTest")
        {
            return new DbContextOptionsBuilder<BankingSystemContext>().UseInMemoryDatabase(dbName)
                                                                      .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                                                                      .Options;
        }

        public void SeedIBANMasterData(BankingSystemContext context)
        {
            if (context == null) return;
            DbInitializer.Initialize(context);
        }

    }
}
