using ES.Framework.Domain.Events;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Events;

/// <summary>Event used when withdrawal was refused because of <see cref="BankAccountState.Balance"/></summary>
public record WithdrawalDeclined : AggregateEvent<BankAccountId>
{
	 /// <summary>Gets or sets the attempted amount.</summary>
	 /// <value>The attempted amount.</value>
	 public decimal AttemptedAmount { get; init; }

	 /// <inheritdoc />
	 public WithdrawalDeclined(BankAccountId id) : base(id) {
	 }
}
