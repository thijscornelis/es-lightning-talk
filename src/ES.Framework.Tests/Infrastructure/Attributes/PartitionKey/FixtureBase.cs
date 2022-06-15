using ES.Framework.Domain.Aggregates.Attributes;
using ES.Sample.Fixtures;

namespace ES.Framework.Tests.Infrastructure.Attributes.PartitionKey;

public abstract class FixtureBase<TClass> : AttributeFixtureBase<TClass, PartitionKeyAttribute>
	where TClass : class
{
}
