using ES.Framework.Domain.Events;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Events;

/// <summary>Event used when money is withdrawn from a <see cref="MoneyWithdrawn" /></summary>
public record MoneyWithdrawn : AggregateEvent<BankAccountId>
{
	 /// <summary>Gets the amount.</summary>
	 /// <value>The amount.</value>
	 public decimal Amount { get; init; }

	 /// <inheritdoc />
	 public MoneyWithdrawn(BankAccountId id) : base(id) {
	 }
}
