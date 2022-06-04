using ES.Framework.Domain.Events;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateDescriptionChanged : AggregateEvent<TestAggregateId>
{
	 public string Description { get; init; }

	 /// <inheritdoc />
	 public TestAggregateDescriptionChanged(TestAggregateId id) : base(id) {
	 }
}
