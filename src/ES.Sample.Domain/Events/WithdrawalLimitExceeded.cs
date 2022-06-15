using ES.Framework.Domain.Events;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Events;

/// <summary>Event used when withdrawal was refused because limit was exceeded</summary>
public record WithdrawalLimitExceeded : AggregateEvent<BankAccountId>
{
	 /// <summary>Gets or sets the attempted amount.</summary>
	 /// <value>The attempted amount.</value>
	 public decimal AttemptedAmount { get; init; }

	 /// <inheritdoc />
	 public WithdrawalLimitExceeded(BankAccountId id) : base(id) {
	 }
}
