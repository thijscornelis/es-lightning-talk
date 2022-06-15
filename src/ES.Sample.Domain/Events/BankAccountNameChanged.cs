using ES.Framework.Domain.Events;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Events;

/// <summary>Event used when <see cref="BankAccount" /> name is changed.</summary>
public record BankAccountNameChanged : AggregateEvent<BankAccountId>
{
	 /// <summary>Gets the name.</summary>
	 /// <value>The name.</value>
	 public string Name { get; init; }

	 /// <inheritdoc />
	 public BankAccountNameChanged(BankAccountId id) : base(id) {
	 }
}
