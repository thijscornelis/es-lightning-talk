using ES.Framework.Domain.Events.Design;

namespace ES.Sample.Domain;

/// <summary>Event used when <see cref="BankAccount" /> is created.</summary>
public record BankAccountCreated : IAggregateEvent<BankAccountId>
{
	 /// <summary>Gets the name.</summary>
	 /// <value>The name.</value>
	 public string Name { get; init; }

	 /// <inheritdoc />
	 public BankAccountId Id { get; init; }

	 /// <inheritdoc />
	 public long Version { get; set; }
}
