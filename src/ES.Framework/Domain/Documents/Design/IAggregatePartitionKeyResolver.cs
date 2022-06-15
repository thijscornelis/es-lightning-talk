using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Abstractions;

/// <summary>Resolve PartitionKeys for aggregates</summary>
public interface IAggregatePartitionKeyResolver
{
	 /// <summary>Creates the synthetic partition key.</summary>
	 /// <param name="aggregateId">The aggregate identifier.</param>
	 /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
	 /// <typeparam name="TKey">The type of the key.</typeparam>
	 /// <typeparam name="TState">The type of the state.</typeparam>
	 /// <typeparam name="TValue">The type of the value.</typeparam>
	 /// <returns>System.String.</returns>
	 public string CreatePartitionKey<TAggregate, TKey, TState, TValue>(TKey aggregateId)
		 where TAggregate : IAggregate<TKey, TState>
		 where TState : class, IAggregateState<TKey>, new()
		 where TKey : ITypedIdentifier<TValue>;
}
