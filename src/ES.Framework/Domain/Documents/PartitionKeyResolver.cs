using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates.Attributes;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Documents;

/// <summary>
///     PartitionKeyResolver that translates the format from <see cref="AggregatePartitionKeyAttribute" /> using a
///     list of arguments to the actual partition key
/// </summary>
public class PartitionKeyResolver : IPartitionKeyResolver
{
	 private readonly IAttributeValueResolver _attributeValueResolver;

	 /// <summary>Initializes a new instance of the <see cref="PartitionKeyResolver" /> class.</summary>
	 /// <param name="attributeValueResolver">The attribute value resolver.</param>
	 public PartitionKeyResolver(IAttributeValueResolver attributeValueResolver) =>
		 _attributeValueResolver = attributeValueResolver;

	 /// <inheritdoc />
	 public string CreateSyntheticPartitionKey<TAggregate, TKey, TState, TValue>(TKey aggregateId)
		 where TAggregate : IAggregate<TKey, TState>
		 where TState : class, IAggregateState<TKey>, new()
		 where TKey : ITypedIdentifier<TValue> {
		  var attribute = _attributeValueResolver.GetValue<AggregatePartitionKeyAttribute>(typeof(TAggregate));
		  return attribute.GetPartitionKey(typeof(TAggregate).Name, aggregateId.TypedValue);
	 }
}