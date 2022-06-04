using ES.Framework.Domain.Events;
using ES.Sample.Domain.Events;
using ES.Sample.Domain.ValueObjects;

namespace ES.Sample.Domain.Aggregates;

public partial class BankAccount
{
	 /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
	 public BankAccount() {
		  Handle<BankAccountCreated>(OnCreated);
		  Handle<BankAccountNameChanged>(OnNameChanged);
		  Handle<MoneyDeposited>(OnDeposited);
		  Handle<MoneyWithdrawn>(OnWithdrawn);

		  Handle<WithdrawalDeclined>(Ignore);
		  Handle<WithdrawalLimitExceeded>(Ignore);
	 }

	 private BankAccountState Ignore(AggregateEvent<BankAccountId> @event) => State;

	 private BankAccountState OnCreated(BankAccountCreated @event) => State with {
		  Id = @event.Id,
		  Name = @event.Name,
		  WithdrawalLimit = new WithdrawalLimit(@event.WithdrawalLimit, TimeSpan.FromMinutes(10))
	 };

	 private BankAccountState OnDeposited(MoneyDeposited @event) => State with { Balance = State.Balance + @event.Amount };

	 private BankAccountState OnNameChanged(BankAccountNameChanged @event) => State with {
		  Id = @event.Id,
		  Name = @event.Name
	 };

	 private BankAccountState OnWithdrawn(MoneyWithdrawn @event) {
		  var balance = State.Balance - @event.Amount;
		  var recentWithdrawals = State.RecentWithdrawals;

		  if(@event.OccurredOn.Add(State.WithdrawalLimit.TimeFrame) > DateTime.UtcNow) {
				recentWithdrawals.Add(new(@event.Amount, @event.OccurredOn));
		  }

		  return State with {
				Balance = balance,
				RecentWithdrawals = recentWithdrawals
		  };
	 }
}
