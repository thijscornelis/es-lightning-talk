using ES.Framework.Domain.Events.Design;

namespace ES.Framework.Tests.Mocks;

public record TestAggregateDescriptionChanged : IAggregateEvent<TestAggregateId>
{
	 public string Description { get; init; }

	 /// <inheritdoc />
	 public TestAggregateId Id { get; init; }
	 /// <inheritdoc />
	 public long Version { get; set; }
}
