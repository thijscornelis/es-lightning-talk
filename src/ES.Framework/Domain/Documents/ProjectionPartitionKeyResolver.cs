using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Aggregates.Attributes;

namespace ES.Framework.Domain.Documents;

/// <inheritdoc />
public class ProjectionPartitionKeyResolver : IProjectionPartitionKeyResolver
{
	 private readonly IAttributeValueResolver _attributeValueResolver;

	 /// <summary>Initializes a new instance of the <see cref="ProjectionPartitionKeyResolver" /> class.</summary>
	 /// <param name="attributeValueResolver">The attribute value resolver.</param>
	 public ProjectionPartitionKeyResolver(IAttributeValueResolver attributeValueResolver) => _attributeValueResolver = attributeValueResolver;

	 /// <inheritdoc />
	 public string CreatePartitionKey<TProjection>() {
		  var attribute = _attributeValueResolver.GetValue<PartitionKeyAttribute>(typeof(TProjection));
		  return attribute.GetPartitionKey(typeof(TProjection).Name);
	 }
}
