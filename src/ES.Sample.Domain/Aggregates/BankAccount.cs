using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Attributes;
using ES.Sample.Domain.Events;
using ES.Sample.Domain.Exceptions;

namespace ES.Sample.Domain.Aggregates;

/// <summary>The <see cref="BankAccount" /> aggregate root.</summary>
[PartitionKey("{0}-{1:N}")]
public partial class BankAccount : Aggregate<BankAccountId, BankAccountState>
{
	 /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
	 /// <param name="name">The name.</param>
	 /// <param name="withdrawalLimit">The withdrawal limit.</param>
	 public BankAccount(string name, decimal withdrawalLimit) : this(BankAccountId.CreateNew(), name, withdrawalLimit) { }

	 /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
	 /// <param name="id">The identifier.</param>
	 /// <param name="name">The name.</param>
	 /// <param name="withdrawalLimit">The withdrawal limit.</param>
	 protected BankAccount(BankAccountId id, string name, decimal withdrawalLimit) : this() {
		  ThrowIfInvalidName(name);
		  Apply(new BankAccountCreated(id) {
				Name = name,
				WithdrawalLimit = withdrawalLimit
		  });
	 }

	 /// <summary>Deposits the specified amount.</summary>
	 /// <param name="amount">The amount.</param>
	 /// <exception cref="ES.Sample.Domain.Exceptions.NegativeAmountException"></exception>
	 /// <exception cref="ES.Sample.Domain.Exceptions.InvalidAmountException"></exception>
	 public void Deposit(decimal amount) {
		  ThrowIfNegativeAmount(amount);
		  ThrowIfInvalidAmount(amount);
		  Apply(new MoneyDeposited(Id) {
				Amount = amount
		  });
	 }

	 /// <summary>Sets the name.</summary>
	 /// <param name="value">The value.</param>
	 /// <exception cref="ArgumentNullException">value</exception>
	 public void SetName(string value) {
		  ThrowIfInvalidName(value);
		  Apply(new BankAccountNameChanged(Id) {
				Name = value
		  });
	 }

	 /// <summary>Withdraws the specified amount.</summary>
	 /// <param name="amount">The amount.</param>
	 /// <exception cref="ES.Sample.Domain.Exceptions.NegativeAmountException"></exception>
	 /// <exception cref="ES.Sample.Domain.Exceptions.InvalidAmountException"></exception>
	 public Exception Withdraw(decimal amount) {
		  ThrowIfNegativeAmount(amount);
		  ThrowIfInvalidAmount(amount);

		  if(WithdrawalExceedsLimit(amount)) {
				Apply(new WithdrawalLimitExceeded(Id) {
					 AttemptedAmount = amount
				});

				return new WithdrawalExceedsLimitException(amount, State.WithdrawalLimit.Amount, State.WithdrawalLimit.TimeFrame);
		  }

		  if(WithdrawalExceedsBalance(amount)) {
				Apply(new WithdrawalDeclined(Id) {
					 AttemptedAmount = amount
				});

				return new WithdrawalExceedsBalanceException(amount);
		  }

		  Apply(new MoneyWithdrawn(Id) {
				Amount = amount
		  });
		  return null;
	 }

	 private static void ThrowIfInvalidAmount(decimal amount) {
		  if(amount == 0)
				throw new InvalidAmountException();
	 }

	 private static void ThrowIfInvalidName(string value) {
		  if(string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(nameof(value));
	 }

	 private static void ThrowIfNegativeAmount(decimal amount) {
		  if(amount < 0)
				throw new NegativeAmountException();
	 }

	 private bool WithdrawalExceedsBalance(decimal amount) => amount > State.Balance;

	 private bool WithdrawalExceedsLimit(decimal amount) {
		  var recentWithdrawals = State.RecentWithdrawals.Sum(x => x.Amount);
		  return recentWithdrawals + amount > State.WithdrawalLimit.Amount;
	 }
}
