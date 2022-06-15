using ES.Framework.Tests.Mocks;
using ES.Sample.Fixtures;

namespace ES.Framework.Tests.Aggregates;

public abstract class FixtureBase : AggregateFixtureBase<TestAggregate, TestAggregateId, TestAggregateState>
{
	 /// <inheritdoc />
	 public FixtureBase() {
	 }

	 /// <inheritdoc />
	 protected override TestAggregate ArrangeAggregate() => new(TestAggregateId.CreateNew(Guid.Parse("{091DAB32-F049-4286-9001-FC963EC4BBF7}")), "TEST AGGREGATE");
}
