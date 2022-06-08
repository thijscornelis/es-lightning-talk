using ES.Framework.Domain.Projections;
using ES.Framework.Domain.Repositories.Design;
using ES.Sample.Domain.Events;
using ES.Sample.Domain.Projections;

namespace ES.Sample.Infrastructure.Projectors;

/// <inheritdoc />
public class BankAccountProjector : Projector<BankAccountProjection>
{
	 /// <inheritdoc />
	 public BankAccountProjector(IProjectionRepository repository) : base(repository) {
		  Handle<BankAccountCreated>(OnCreated, e => BankAccountProjection.GetProjectionId(e.Id));
		  Handle<BankAccountNameChanged>(OnNameChanged, e => BankAccountProjection.GetProjectionId(e.Id));
		  Handle<MoneyWithdrawn>(OnWithdraw, e => BankAccountProjection.GetProjectionId(e.Id));
		  Handle<MoneyDeposited>(OnDeposit, e => BankAccountProjection.GetProjectionId(e.Id));
	 }

	 private BankAccountProjection OnCreated(BankAccountCreated @event, BankAccountProjection projection) => projection
	with {
		  Balance = 0,
		  Name = @event.Name,
		  NumberOfDeposits = 0,
		  NumberOfWithdrawals = 0,
		  BankAccountId = @event.Id
	 };

	 private BankAccountProjection OnDeposit(MoneyDeposited @event, BankAccountProjection projection) =>
		projection with { Balance = projection.Balance + @event.Amount, NumberOfDeposits = projection.NumberOfDeposits + 1 };

	 private BankAccountProjection OnNameChanged(BankAccountNameChanged @event, BankAccountProjection projection) =>
	projection with { Name = @event.Name };

	 private BankAccountProjection OnWithdraw(MoneyWithdrawn @event, BankAccountProjection projection) =>
		projection with { Balance = projection.Balance - @event.Amount, NumberOfWithdrawals = projection.NumberOfWithdrawals + 1 };
}
