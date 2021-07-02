using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;
        public const decimal BalanceLow = 500m;
        public const decimal ApproachingPayInLimit = 500m;

        public Account(Guid id, User user,
            decimal balance, decimal withdrawn, decimal paidIn)
        {
            Id = id;
            User = user;
            Balance = balance;
            Withdrawn = withdrawn;
            PaidIn = paidIn;
        }

        public Guid Id { get; private set; }

        public User User { get; private set; }

        public decimal Balance { get; private set; }

        public decimal Withdrawn { get; private set; }

        public decimal PaidIn { get; private set; }

        /// <summary>
        /// Withdraw money from the account
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="InvalidOperationException">Thrown when account has insufficient funds to withdraw</exception>
        public void WithdrawMoney(decimal amount)
        {
            var finalBalance = Balance - amount;
            if (finalBalance < 0m)
            {
                throw new InvalidOperationException("Account - insufficient funds to withdraw");
            }

            Balance -= amount;
            Withdrawn -= amount;
        }

        /// <summary>
        /// Pay in money to the account
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="InvalidOperationException">Thrown when account pay in limit reached</exception>
        public void PayInMoney(decimal amount)
        {
            var paidIn = PaidIn + amount;
            if (paidIn > PayInLimit)
            {
                throw new InvalidOperationException("Account - pay in limit reached");
            }

            Balance += amount;
            PaidIn += amount;
        }

        /// <summary>
        /// Check if account balance low
        /// </summary>
        public bool CheckIfBalanceLow()
        {
            return Balance < BalanceLow;
        }

        /// <summary>
        /// Check if account approaching pay in limit
        /// </summary>
        public bool CheckIfApproachingPayInLimit()
        {
            return PayInLimit - PaidIn < ApproachingPayInLimit;
        }
    }
}
