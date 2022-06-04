using ES.Framework.Domain.Events.Design;

namespace ES.Sample.Domain;

/// <summary>Event used when <see cref="BankAccount" /> name is changed.</summary>
public record BankAccountNameChanged : IAggregateEvent<BankAccountId>
{
	 /// <inheritdoc />
	 public BankAccountId Id { get; init; }

	 /// <inheritdoc />
	 public long Version { get; set; }

	 /// <summary>Gets the name.</summary>
	 /// <value>The name.</value>
	 public string Name { get; init; }
}
