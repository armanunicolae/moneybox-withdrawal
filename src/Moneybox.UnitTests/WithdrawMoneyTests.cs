using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;
using System;

namespace Moneybox.UnitTests
{
    public class WithdrawMoneyTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Account_WithdrawMoney_Should_Set_Correct_Values()
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

            accountRepositoryMock
                .Setup(x => x.GetAccountById(fromAccountId))
                .Returns(fromAccount);

            var withdrawMoneyService = new WithdrawMoney(accountRepositoryMock.Object, notificationServiceMock.Object);

            decimal expectedFromAccountBalance = 450;
            decimal expectedFromAccountWithdrawn = 50;
            decimal expectedFromAccountPaidIn = 0;

            // Act
            withdrawMoneyService.Execute(fromAccountId, 50);

            // Assert
            Assert.AreEqual(expectedFromAccountBalance, fromAccount.Balance);
            Assert.AreEqual(expectedFromAccountWithdrawn, fromAccount.Withdrawn);
            Assert.AreEqual(expectedFromAccountPaidIn, fromAccount.PaidIn);

            notificationServiceMock.Verify(m => m.NotifyFundsLow(fromAccount.User.Email), Times.Once);
            accountRepositoryMock.Verify(m => m.Update(fromAccount), Times.Once);
        }
    }
}