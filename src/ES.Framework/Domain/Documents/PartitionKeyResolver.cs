using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Attributes;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Documents;

/// <summary>PartitionKeyResolver that translates the format from <see cref="AggregatePartitionKeyAttribute" /> using a list of arguments to the actual partition key</summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TState">The type of the state.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class PartitionKeyResolver<TAggregate, TKey, TState, TValue> : IPartitionKeyResolver<TAggregate, TKey, TState, TValue>
	 where TAggregate : Aggregate<TKey, TState>
	 where TState : class, IAggregateState<TKey>, new()
	 where TKey : ITypedIdentifier<TValue>

{
	 private readonly IAttributeValueResolver _attributeValueResolver;

	 /// <summary>Initializes a new instance of the <see cref="PartitionKeyResolver{TAggregate, TKey, TState, TValue}" /> class.</summary>
	 /// <param name="attributeValueResolver">The attribute value resolver.</param>
	 public PartitionKeyResolver(IAttributeValueResolver attributeValueResolver) => _attributeValueResolver = attributeValueResolver;

	 /// <inheritdoc />
	 public string CreateSyntheticPartitionKey(TKey aggregateId) {
		  var attribute = _attributeValueResolver.GetValue<AggregatePartitionKeyAttribute>(typeof(TAggregate));
		  return attribute.GetPartitionKey(typeof(TAggregate).Name, aggregateId.TypedValue);
	 }
}
