using ES.Framework.Domain.Events;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Events;

/// <summary>Event used when money is deposited into a <see cref="BankAccount" /></summary>
public record MoneyDeposited : AggregateEvent<BankAccountId>
{
	 /// <summary>Gets the amount.</summary>
	 /// <value>The amount.</value>
	 public decimal Amount { get; init; }

	 /// <summary>Gets the description.</summary>
	 /// <value>The description.</value>
	 public string Description { get; init; }

	 /// <summary>Gets the statement identifier.</summary>
	 /// <value>The statement identifier.</value>
	 public StatementId StatementId { get; init; }
	 /// <inheritdoc />
	 public MoneyDeposited(BankAccountId id) : base(id) {
	 }
}
