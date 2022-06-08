using ES.Framework.Domain.Projections;
using ES.Framework.Domain.Repositories.Design;
using ES.Sample.Domain.Events;
using ES.Sample.Domain.Projections;

namespace ES.Sample.Infrastructure.Projectors;

/// <inheritdoc />
public class BankStatementProjector : Projector<BankStatementProjection>
{
	 /// <inheritdoc />
	 public BankStatementProjector(IProjectionRepository repository) : base(repository) {
		  Handle<MoneyWithdrawn>(OnWithdraw, e => BankStatementProjection.GetProjectionId(e.Id));
		  Handle<MoneyDeposited>(OnDeposit, e => BankStatementProjection.GetProjectionId(e.Id));
	 }

	 private BankStatementProjection OnDeposit(MoneyDeposited @event, BankStatementProjection projection) =>
		  projection with { Amount = @event.Amount, On = @event.OccurredOn, BankAccountId = @event.Id };

	 private BankStatementProjection OnWithdraw(MoneyWithdrawn @event, BankStatementProjection projection) =>
		 projection with { Amount = @event.Amount * -1, On = @event.OccurredOn, BankAccountId = @event.Id };
}
