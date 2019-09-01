using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public static class Constant
    {
        public const decimal Fee_Percent = 0.1M;

        #region Error messages
        public const string ACCOUNT_NOT_FOUND = "Account not found.";
        public const string CUSTOMER_IS_NULL = "Customer can't be null.";
        public const string NO_IBAN_LEFT = "No IBAN number left.";
        public const string IBAN_IS_NULL = "IBAN number can't be null.";
        public const string DEPOSIT_ZERO_MONEY = "Deposit money must more than zero.";
        #endregion
    }
}
