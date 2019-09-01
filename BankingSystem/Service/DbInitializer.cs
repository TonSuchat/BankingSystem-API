using Entity.DBModels;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public static class DbInitializer
    {
        // initial IBAN master data for using in api
        public static void Initialize(BankingSystemContext context)
        {
            context.Database.EnsureCreated();

            if (context.MasterIBANs.Any()) return; // IBAN has been seeded

            List<MasterIBAN> masterIBANs = new List<MasterIBAN>()
            {
                new MasterIBAN() { IBAN  = "NL92ABNA8946051078", Used = false },
                new MasterIBAN() { IBAN  = "NL05INGB6289099205", Used = false },
                new MasterIBAN() { IBAN  = "NL35ABNA7806242643", Used = false },
                new MasterIBAN() { IBAN  = "NL82RABO7030136047", Used = false },
                new MasterIBAN() { IBAN  = "NL34ABNA6918258974", Used = false },
                new MasterIBAN() { IBAN  = "NL80INGB7434587830", Used = false },
                new MasterIBAN() { IBAN  = "NL07ABNA5350244469", Used = false },
                new MasterIBAN() { IBAN  = "NL66ABNA1272753638", Used = false },
                new MasterIBAN() { IBAN  = "NL50RABO8386803843", Used = false },
                new MasterIBAN() { IBAN  = "NL12RABO5111110259", Used = false },
                new MasterIBAN() { IBAN  = "NL47ABNA3627647424", Used = false },
                new MasterIBAN() { IBAN  = "NL30RABO8877477636", Used = false },
                new MasterIBAN() { IBAN  = "NL10INGB9763136946", Used = false },
                new MasterIBAN() { IBAN  = "NL64INGB8247360527", Used = false },
                new MasterIBAN() { IBAN  = "NL59ABNA2825057568", Used = false },
                new MasterIBAN() { IBAN  = "NL38ABNA4824538831", Used = false },
                new MasterIBAN() { IBAN  = "NL80INGB6371362585", Used = false },
                new MasterIBAN() { IBAN  = "NL27INGB2760150151", Used = false },
                new MasterIBAN() { IBAN  = "NL02ABNA5136679077", Used = false },
                new MasterIBAN() { IBAN  = "NL06INGB2764204256", Used = false },
            };

            context.MasterIBANs.AddRange(masterIBANs);

            context.SaveChanges();
        }
    }
}
