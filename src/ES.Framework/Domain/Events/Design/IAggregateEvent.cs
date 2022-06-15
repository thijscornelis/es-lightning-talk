using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Events.Design;

/// <summary>Interface IAggregateEvent Implements the <see cref="ES.Framework.Domain.Events.Design.IEvent" /></summary>
/// <typeparam name="TKey">The type of the aggregate identifier.</typeparam>
/// <seealso cref="ES.Framework.Domain.Events.Design.IEvent" />
public interface IAggregateEvent<TKey> : IEvent
	 where TKey : ITypedIdentifier
{
	 /// <summary>Gets the aggregate identifier.</summary>
	 /// <value>The identifier.</value>
	 public TKey Id { get; init; }

	 /// <summary>Gets the version.</summary>
	 /// <value>The version.</value>
	 public long Version { get; }
}
