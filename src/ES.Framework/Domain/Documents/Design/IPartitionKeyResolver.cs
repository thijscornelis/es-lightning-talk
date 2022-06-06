using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Abstractions;

/// <summary>Resolve PartitionKeys for aggregates</summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TState">The type of the state.</typeparam>
/// <typeparam name="TValue">The type of the t value.</typeparam>
public interface IPartitionKeyResolver<TAggregate, in TKey, TState, TValue>
	where TAggregate : IAggregate<TKey, TState>
	where TState : class, IAggregateState<TKey>, new()
	where TKey : ITypedIdentifier<TValue>
{
	 /// <summary>Creates the synthetic partition key.</summary>
	 /// <param name="aggregateId">The aggregate identifier.</param>
	 /// <returns>System.String.</returns>
	 public string CreateSyntheticPartitionKey(TKey aggregateId);
}
