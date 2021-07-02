using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;
using System;

namespace Moneybox.UnitTests
{
    public class TransferMoneyTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Account_TransferMoney_Should_Set_Correct_Values()
        {
            // Arrange
            var notificationServiceMock = new Mock<INotificationService>();
            var accountRepositoryMock = new Mock<IAccountRepository>();

            var fromAccountId = Guid.NewGuid();
            var fromAccount = new Account(
                id: fromAccountId,
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: 500,
                withdrawn: 100,
                paidIn: 0
            );

            var toAccountId = Guid.NewGuid();
            var toAccount = new Account(
                id: toAccountId,
                user: new User(Guid.NewGuid(), "John", "john@example.com"),
                balance: 1000,
                withdrawn: 100,
                paidIn: 0
            );

            accountRepositoryMock
                .Setup(x => x.GetAccountById(fromAccountId))
                .Returns(fromAccount);

            accountRepositoryMock
                .Setup(x => x.GetAccountById(toAccountId))
                .Returns(toAccount);

            var transferMoneyService = new TransferMoney(accountRepositoryMock.Object, notificationServiceMock.Object);

            decimal expectedFromAccountBalance = 450;
            decimal expectedFromAccountWithdrawn = 50;
            decimal expectedFromAccountPaidIn = 0;
            decimal expectedToAccountBalance = 1050;
            decimal expectedToAccountWithdrawn = 100;
            decimal expectedToAccountPaidIn = 50;

            // Act
            transferMoneyService.Execute(fromAccountId, toAccountId, 50);

            // Assert
            Assert.AreEqual(expectedFromAccountBalance, fromAccount.Balance);
            Assert.AreEqual(expectedFromAccountWithdrawn, fromAccount.Withdrawn);
            Assert.AreEqual(expectedFromAccountPaidIn, fromAccount.PaidIn);
            Assert.AreEqual(expectedToAccountBalance, toAccount.Balance);
            Assert.AreEqual(expectedToAccountWithdrawn, toAccount.Withdrawn);
            Assert.AreEqual(expectedToAccountPaidIn, toAccount.PaidIn);

            notificationServiceMock.Verify(m => m.NotifyFundsLow(fromAccount.User.Email), Times.Once);
            notificationServiceMock.Verify(m => m.NotifyApproachingPayInLimit(toAccount.User.Email), Times.Never);
            accountRepositoryMock.Verify(m => m.Update(fromAccount), Times.Once);
            accountRepositoryMock.Verify(m => m.Update(toAccount), Times.Once);
        }
    }
}