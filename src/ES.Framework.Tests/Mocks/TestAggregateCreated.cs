using ES.Framework.Domain.Events;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateCreated : AggregateEvent<TestAggregateId>
{
	 public string Name { get; init; }

	 /// <inheritdoc />
	 public TestAggregateCreated(TestAggregateId id) : base(id) {
	 }
}
