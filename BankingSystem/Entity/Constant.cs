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
        public const string TRANSFER_ZERO_MONEY = "Transfer money must more than zero.";
        public const string NOT_FOUND_TRANSFER_ACCOUNT = "Not found transfer account.";
        public const string NOT_FOUND_RECEIVE_ACCOUNT = "Not found receive account.";
        public const string TRANSFER_MONEY_NOT_ENOUGH = "Transfer money not enough.";
        #endregion
    }
}
