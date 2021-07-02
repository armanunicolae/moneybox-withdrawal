using Moneybox.App;
using NUnit.Framework;
using System;

namespace Moneybox.UnitTests
{
    public class AccountTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Account_WithdrawMoney_Should_Set_Correct_Values()
        {
            // Arrange
            var account = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: 1000,
                withdrawn: 100,
                paidIn: 0
            );
            decimal expectedBalance = 950;
            decimal expectedWithdrawn = 50;

            // Act
            account.WithdrawMoney(50);

            // Assert
            Assert.AreEqual(expectedBalance, account.Balance);
            Assert.AreEqual(expectedWithdrawn, account.Withdrawn);
        }

        [Test]
        public void Account_WithdrawMoney_Should_Throw_Exception_When_Insufficient_Funds_To_Withdraw()
        {
            // Arrange
            var account = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: 1000,
                withdrawn: 100,
                paidIn: 0
            );

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => account.WithdrawMoney(1100));
        }

        [Test]
        public void Account_PayInMoney_Should_Set_Correct_Values()
        {
            // Arrange
            var account = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: 1000,
                withdrawn: 100,
                paidIn: 0
            );
            decimal expectedBalance = 1050;
            decimal expectedPaidIn = 50;

            // Act
            account.PayInMoney(50);

            // Assert
            Assert.AreEqual(expectedBalance, account.Balance);
            Assert.AreEqual(expectedPaidIn, account.PaidIn);
        }

        [Test]
        public void Account_PayInMoney_Should_Throw_Exception_When_Pay_In_Limit_Reached()
        {
            // Arrange
            var account = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: 1000,
                withdrawn: 100,
                paidIn: 0
            );
            var amountToPay = Account.PayInLimit + 100;

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => account.PayInMoney(amountToPay));
        }

        [Test]
        public void Account_CheckIfBalanceLow_Should_ReturnTrue()
        {
            // Arrange
            var accountBalanceLow = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: Account.BalanceLow - 1,
                withdrawn: 100,
                paidIn: 0
            );

            // Act
            var resultBalanceLow = accountBalanceLow.CheckIfBalanceLow();

            // Assert
            Assert.AreEqual(true, resultBalanceLow);
        }

        [Test]
        public void Account_CheckIfBalanceLow_Should_ReturnFalse()
        {
            // Arrange
            var accountBalanceOk = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: Account.BalanceLow,
                withdrawn: 100,
                paidIn: 0
            );

            // Act
            var resultBalanceOk = accountBalanceOk.CheckIfBalanceLow();

            // Assert
            Assert.AreEqual(false, resultBalanceOk);
        }

        [Test]
        public void Account_CheckIfApproachingPayInLimit_Should_ReturnTrue()
        {
            // Arrange
            var accountApproachingPayInLimit = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: Account.PayInLimit - Account.ApproachingPayInLimit + 1,
                withdrawn: 0,
                paidIn: Account.PayInLimit - Account.ApproachingPayInLimit + 1
            );

            // Act
            var resultApproachingPayInLimit = accountApproachingPayInLimit.CheckIfApproachingPayInLimit();

            // Assert
            Assert.AreEqual(true, resultApproachingPayInLimit);
        }

        [Test]
        public void Account_CheckIfApproachingPayInLimit_Should_ReturnFalse()
        {
            // Arrange
            var accountOk = new Account(
                id: Guid.NewGuid(),
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: Account.PayInLimit - Account.ApproachingPayInLimit,
                withdrawn: 0,
                paidIn: Account.PayInLimit - Account.ApproachingPayInLimit
            );

            // Act
            var resultOk = accountOk.CheckIfApproachingPayInLimit();

            // Assert
            Assert.AreEqual(false, resultOk);
        }
    }
}