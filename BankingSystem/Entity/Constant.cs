using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public static class Constant
    {
        public const decimal Fee_Percent = 0.1M;

        #region Error messages
        public const string CREATEACCOUNT_CUSTOMER_IS_NULL = "Customer can't be null.";
        public const string CREATEACCOUNT_NO_IBAN_LEFT = "No IBAN number left.";
        #endregion
    }
}
