using ES.Framework.Domain.Events;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Events;

/// <summary>Event used when <see cref="BankAccount" /> is created.</summary>
public record BankAccountCreated : AggregateEvent<BankAccountId>
{
	 /// <summary>Gets the name.</summary>
	 /// <value>The name.</value>
	 public string Name { get; init; }

	 /// <summary>Gets or sets the withdrawal limit.</summary>
	 /// <value>The withdrawal limit.</value>
	 public decimal WithdrawalLimit { get; init; }

	 /// <inheritdoc />
	 public BankAccountCreated(BankAccountId id) : base(id) {
	 }
}
