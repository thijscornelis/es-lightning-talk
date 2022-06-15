using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates.Attributes;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Documents;

/// <summary>PartitionKeyResolver that translates the format from <see cref="PartitionKeyAttribute" /> using a list of arguments to the actual partition key</summary>
public class AggregatePartitionKeyResolver : IAggregatePartitionKeyResolver
{
	 private readonly IAttributeValueResolver _attributeValueResolver;

	 /// <summary>Initializes a new instance of the <see cref="AggregatePartitionKeyResolver" /> class.</summary>
	 /// <param name="attributeValueResolver">The attribute value resolver.</param>
	 public AggregatePartitionKeyResolver(IAttributeValueResolver attributeValueResolver) =>
		 _attributeValueResolver = attributeValueResolver;

	 /// <inheritdoc />
	 public string CreatePartitionKey<TAggregate, TKey, TState, TValue>(TKey aggregateId)
		 where TAggregate : IAggregate<TKey, TState>
		 where TState : class, IAggregateState<TKey>, new()
		 where TKey : ITypedIdentifier<TValue> {
		  var attribute = _attributeValueResolver.GetValue<PartitionKeyAttribute>(typeof(TAggregate));
		  return attribute.GetPartitionKey(typeof(TAggregate).Name, aggregateId.TypedValue);
	 }
}
